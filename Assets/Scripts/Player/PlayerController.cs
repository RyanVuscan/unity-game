using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireCooldown = 0.15f;


    Rigidbody2D rb;
    PlayerElement element;
    float lastShot;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        element = GetComponent<PlayerElement>();
    }


    void Update()
    {
        // Swap element
        if (Input.GetKeyDown(KeyCode.Space)) element.NextElement();


        // Shoot
        if (Input.GetKey(KeyCode.G) && Time.time - lastShot >= fireCooldown)
        {
            lastShot = Time.time;
            Shoot();
        }
    }


    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y).normalized;
        rb.linearVelocity = dir * moveSpeed;


        // Face movement direction if moving
        if (dir.sqrMagnitude > 0.01f)
            transform.up = dir; // top of sprite points toward movement
    }


    void Shoot()
    {
        var go = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
        if (go == null)
        {
            Debug.LogError("Shoot(): projectilePrefab is null!");
            return;
        }

        var proj = go.GetComponent<Projectile>();
        if (proj == null)
        {
            Debug.LogError("Shoot(): No Projectile component found on " + go.name);
            return;
        }

        if (element == null)
        {
            Debug.LogError("Shoot(): element reference is null on PlayerController!");
            return;
        }

        proj.Init(owner: gameObject, elementType: element.Current);
    }
}