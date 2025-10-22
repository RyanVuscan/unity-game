using UnityEngine;

public class AttackState : State
{
    Transform player;
    Transform firePoint;
    GameObject projectilePrefab;
    float fireInterval;
    float lastShot;

    public AttackState(GameObject owner, StateMachine fsm, Transform player, 
                       Transform firePoint, GameObject projectilePrefab, float fireInterval)
        : base(owner, fsm)
    {
        this.player = player;
        this.firePoint = firePoint;
        this.projectilePrefab = projectilePrefab;
        this.fireInterval = fireInterval;
    }

    public override void Update()
    {
        if (player == null) return;

        // Face player
        Vector2 dir = (player.position - owner.transform.position).normalized;
        owner.transform.up = dir;

        float dist = Vector2.Distance(owner.transform.position, player.position);
        if (dist > 3f)
            fsm.ChangeState(new ChaseState(owner, fsm, player, 2f, 3f));

        if (Time.time - lastShot >= fireInterval)
        {
            lastShot = Time.time;
            var go = Object.Instantiate(projectilePrefab, firePoint.position, owner.transform.rotation);
            if (go.TryGetComponent(out Projectile proj))
                proj.Init(owner, ElementType.Fire);
        }
    }
}
