using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    Transform player;
    float speed, detectionRange;
    NavMeshAgent agent;

    public IdleState(GameObject owner, StateMachine fsm, Transform player, float speed, float detectionRange)
        : base(owner, fsm)
    {
        this.player = player;
        this.speed = speed;
        this.detectionRange = detectionRange;
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

        float dist = Vector3.Distance(owner.transform.position, player.position);
        if (dist < detectionRange)
            fsm.ChangeState(new ChaseState(owner, fsm, player, speed, detectionRange));
    }
}
