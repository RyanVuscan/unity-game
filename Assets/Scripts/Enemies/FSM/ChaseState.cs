using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    Transform player;
    float moveSpeed, attackRange;
    NavMeshAgent agent;

    public ChaseState(GameObject owner, StateMachine fsm, Transform player, float moveSpeed, float attackRange)
        : base(owner, fsm)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.attackRange = attackRange;
        agent = owner.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.speed = moveSpeed;
        }
    }

    public override void Update()
    {
        if (player == null || agent == null) return;

        float dist = Vector3.Distance(owner.transform.position, player.position);
        
        // Player got too far away, go back to what we were doing
        if (dist > moveSpeed * 3f)
        {
            var ai = owner.GetComponent<ElementalEnemyAI>();
            
            if (ai != null && ai.UsePatrol && ai.Waypoints != null && ai.Waypoints.Length > 0)
            {
                fsm.ChangeState(new PatrolState(owner, fsm, player, ai.Waypoints, ai.MoveSpeed, ai.DetectionRange));
            }
            else
            {
                fsm.ChangeState(new IdleState(owner, fsm, player, moveSpeed, moveSpeed * 2f));
            }
            return;
        }

        // Follow the player using navmesh
        agent.SetDestination(player.position);

        if (agent.velocity.sqrMagnitude > 0.1f)
            owner.transform.forward = agent.velocity.normalized;

        // Switch to attack if close enough
        if (dist <= attackRange)
        {
            var ai = owner.GetComponent<ElementalEnemyAI>();
            fsm.ChangeState(new AttackState(owner, fsm, player, ai.FirePoint, ai.ProjectilePrefab, ai.FireInterval, ai.AttackRange));
        }
    }

    public override void Exit()
    {
        if (agent != null)
            agent.isStopped = true;
    }
}
