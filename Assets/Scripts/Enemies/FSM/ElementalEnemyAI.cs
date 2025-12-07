using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class ElementalEnemyAI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float detectionRange = 8f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float fireInterval = 1.2f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    [Header("Patrol")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] bool usePatrol = false;

    // public getters
    public Transform FirePoint => firePoint;
    public GameObject ProjectilePrefab => projectilePrefab;
    public float FireInterval => fireInterval;
    public NavMeshAgent Agent { get; private set; }
    public bool UsePatrol => usePatrol;
    public Transform[] Waypoints => waypoints;
    public float MoveSpeed => moveSpeed;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;

    Transform player;
    StateMachine fsm;

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = moveSpeed;
        Agent.stoppingDistance = attackRange;
        Agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        Agent.avoidancePriority = 50;
        Agent.radius = 0.5f;
    }

    void Start()
    {
        // Try to find player, retry if not found yet
        TryInitialize();
    }

    void TryInitialize()
    {
        // Find player in Start instead of Awake to give time for network spawning
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Invoke("TryInitialize", 0.5f); // Retry every half second
            return;
        }
        
        player = playerObj.transform;
        fsm = new StateMachine();

        // Start with patrol if enabled, otherwise just idle
        if (usePatrol && waypoints != null && waypoints.Length > 0)
        {
            fsm.ChangeState(new PatrolState(gameObject, fsm, player, waypoints, moveSpeed, detectionRange));
        }
        else
        {
            fsm.ChangeState(new IdleState(gameObject, fsm, player, moveSpeed, detectionRange));
        }
    }

    void Update()
    {
        if (fsm != null)
            fsm.Update();
    }
}
