using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform[] waypoints;
    int currentIndex = 0;

    [Header("Shooting")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireInterval = 1.5f;
    float lastShot;

    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Vector2 target = waypoints[currentIndex].position;
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        transform.up = dir;

        if (Vector2.Distance(transform.position, target) < 0.1f)
            currentIndex = (currentIndex + 1) % waypoints.Length;
    }

    void Update()
    {
        if (Time.time - lastShot >= fireInterval)
        {
            lastShot = Time.time;
            Shoot();
        }
    }

    void Shoot()
    {
        var go = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        if (go.TryGetComponent(out Projectile proj))
            proj.Init(gameObject, ElementType.Fire);   // temp element
    }
}