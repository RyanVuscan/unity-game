using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 25f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireCooldown = 0.15f;

    Rigidbody rb;
    PlayerElement element;
    float lastShot;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        element = GetComponent<PlayerElement>();
        Debug.Log("PlayerController Awake called");
    }

    void Start()
    {
        Debug.Log($"PlayerController Start - IsSpawned: {IsSpawned}, IsOwner: {IsOwner}");
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(">>> OnNetworkSpawn CALLED <<<");
        base.OnNetworkSpawn();
        
        if (IsOwner)
        {
            rb.isKinematic = false;
            Debug.Log($"I am the OWNER - can move! ClientId: {OwnerClientId}");
        }
        else
        {
            rb.isKinematic = true;
            Debug.Log($"NOT owner - controlled by client {OwnerClientId}");
        }
    }

    bool CanControl()
    {
        // If not network spawned, allow control (offline mode)
        if (!IsSpawned) return true;
        // If network spawned, only owner can control
        return IsOwner;
    }

    void Update()
    {
        if (!CanControl()) return;

        // Debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"Player: IsSpawned={IsSpawned}, IsOwner={IsOwner}, rb.isKinematic={rb.isKinematic}");
        }

        // Swap element
        if (Input.GetKeyDown(KeyCode.Space)) 
            element.NextElement();

        // Shoot
        if (Input.GetKey(KeyCode.G) && Time.time - lastShot >= fireCooldown)
        {
            lastShot = Time.time;
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (!CanControl()) return;
        
        // Make sure we can actually move
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x, 0f, z).normalized;
        
        rb.linearVelocity = new Vector3(dir.x * moveSpeed, 0f, dir.z * moveSpeed);

        if (dir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

    void Shoot()
    {
        if (IsSpawned)
        {
            ShootServerRpc(firePoint.position, transform.rotation, (int)element.Current);
        }
        else
        {
            SpawnProjectile(firePoint.position, transform.rotation, element.Current);
        }
    }

    [ServerRpc]
    void ShootServerRpc(Vector3 position, Quaternion rotation, int elementType)
    {
        SpawnProjectile(position, rotation, (ElementType)elementType);
    }

    void SpawnProjectile(Vector3 position, Quaternion rotation, ElementType elementType)
    {
        var go = Instantiate(projectilePrefab, position, rotation);
        if (go == null) return;

        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(owner: gameObject, elementType: elementType);
        }
    }
}