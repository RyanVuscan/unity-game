using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    Transform player;
    Transform[] waypoints;
    int currentWaypointIndex = 0;
    float detectionRange;
    float waypointReachThreshold = 1.5f;
    NavMeshAgent agent;

    public PatrolState(GameObject owner, StateMachine fsm, Transform player, Transform[] waypoints, float moveSpeed, float detectionRange)
        : base(owner, fsm)
    {
        this.player = player;
        this.waypoints = waypoints;
        this.detectionRange = detectionRange;
        agent = owner.GetComponent<NavMeshAgent>();
        
        if (agent != null)
            agent.speed = moveSpeed;
    }

    public override void Enter()
    {
        if (agent != null)
        {
            agent.isStopped = false;
            
            if (waypoints != null && waypoints.Length > 0)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    public override void Update()
    {
        if (agent == null) return;

        // Check for player detection
        if (player != null)
        {
            float distToPlayer = Vector3.Distance(owner.transform.position, player.position);
            if (distToPlayer < detectionRange)
            {
                // Player detected! Switch to chase
                fsm.ChangeState(new ChaseState(owner, fsm, player, agent.speed, detectionRange * 0.5f));
                return;
            }
        }

        // Continue patrolling if waypoints exist
        if (waypoints == null || waypoints.Length == 0)
        {
            // No waypoints - just idle in place
            agent.isStopped = true;
            return;
        }

        // Check if NavMesh path is valid
        if (agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError($"Invalid NavMesh path to waypoint {currentWaypointIndex}! NavMesh might not be baked properly.");
        }

        // Check if we reached the current waypoint
        float distToWaypoint = Vector3.Distance(owner.transform.position, waypoints[currentWaypointIndex].position);
        if (distToWaypoint < waypointReachThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        // Face movement direction
        if (agent.velocity.sqrMagnitude > 0.1f)
            owner.transform.forward = agent.velocity.normalized;
    }

    public override void Exit()
    {
        if (agent != null)
            agent.isStopped = true;
    }
}
