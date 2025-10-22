using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class ElementalEnemyAI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float detectionRange = 4f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float fireInterval = 1.2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    // public getters (not sure why needed)
    public Transform FirePoint => firePoint;
    public GameObject ProjectilePrefab => projectilePrefab;
    public float FireInterval => fireInterval;

    Rigidbody2D rb;
    Transform player;
    StateMachine fsm;

    // Concrete states
    IdleState idle;
    ChaseState chase;
    AttackState attack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        fsm = new StateMachine();

        idle   = new IdleState(gameObject, fsm, player, moveSpeed, detectionRange);
        chase  = new ChaseState(gameObject, fsm, player, moveSpeed, attackRange);
        attack = new AttackState(gameObject, fsm, player, firePoint, projectilePrefab, fireInterval);

        fsm.ChangeState(idle);
    }

    void Update() => fsm.Update();
}
