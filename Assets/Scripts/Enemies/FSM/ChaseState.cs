using UnityEngine;

public class ChaseState : State
{
    Transform player;
    float moveSpeed, attackRange;
    Rigidbody2D rb;

    public ChaseState(GameObject owner, StateMachine fsm, Transform player, float moveSpeed, float attackRange)
        : base(owner, fsm)
    {
        this.player = player;
        this.moveSpeed = moveSpeed;
        this.attackRange = attackRange;
        rb = owner.GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        if (player == null) return;

        Vector2 dir = (player.position - owner.transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        owner.transform.up = dir;

        float dist = Vector2.Distance(owner.transform.position, player.position);
        if (dist <= attackRange)
            fsm.ChangeState(new AttackState(owner, fsm, player, 
                owner.GetComponent<ElementalEnemyAI>().FirePoint,
                owner.GetComponent<ElementalEnemyAI>().ProjectilePrefab,
                owner.GetComponent<ElementalEnemyAI>().FireInterval));
    }
}
