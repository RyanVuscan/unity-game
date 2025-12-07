using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    Transform player;
    Transform firePoint;
    GameObject projectilePrefab;
    float fireInterval;
    float attackRange;
    float lastShot;
    NavMeshAgent agent;

    public AttackState(GameObject owner, StateMachine fsm, Transform player, 
                       Transform firePoint, GameObject projectilePrefab, float fireInterval, float attackRange)
        : base(owner, fsm)
    {
        this.player = player;
        this.firePoint = firePoint;
        this.projectilePrefab = projectilePrefab;
        this.fireInterval = fireInterval;
        this.attackRange = attackRange;
        agent = owner.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        if (agent != null)
            agent.isStopped = true;
    }

    public override void Update()
    {
        if (player == null) return;

        // Always face the player
        Vector3 dir = (player.position - owner.transform.position).normalized;
        owner.transform.forward = dir;

        float dist = Vector3.Distance(owner.transform.position, player.position);
        
        // Check if we have line of sight to the player
        bool hasLineOfSight = false;
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, dir, out hit, dist))
        {
            // If raycast hits the player, we have line of sight
            if (hit.transform == player)
                hasLineOfSight = true;
        }
        
        // Player moved away OR no line of sight, chase them
        if (dist > attackRange * 1.5f || !hasLineOfSight)
        {
            var ai = owner.GetComponent<ElementalEnemyAI>();
            fsm.ChangeState(new ChaseState(owner, fsm, player, ai.MoveSpeed, ai.AttackRange));
            return;
        }

        // Fire projectile at interval
        if (Time.time - lastShot >= fireInterval)
        {
            lastShot = Time.time;
            
            // Get element type from enemy component
            ElementType enemyElement = ElementType.Fire;
            if (owner.TryGetComponent(out ElementalAffinity affinity))
                enemyElement = affinity.Element;
            
            var go = Object.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            if (go.TryGetComponent(out Projectile proj))
                proj.Init(owner, enemyElement);
        }
    }
}
