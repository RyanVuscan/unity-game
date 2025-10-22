using UnityEngine;

public class IdleState : State
{
    Transform player;
    float speed, detectionRange;
    Rigidbody2D rb;

    public IdleState(GameObject owner, StateMachine fsm, Transform player, float speed, float detectionRange)
        : base(owner, fsm)
    {
        this.player = player;
        this.speed = speed;
        this.detectionRange = detectionRange;
        rb = owner.GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(owner.transform.position, player.position);
        if (dist < detectionRange)
            fsm.ChangeState(new ChaseState(owner, fsm, player, speed, detectionRange));
    }
}
