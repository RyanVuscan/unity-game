using UnityEngine;


public class Projectile : MonoBehaviour
{
[SerializeField] float speed = 14f;
[SerializeField] float baseDamage = 10f;
[SerializeField] float lifetime = 2.5f;
[SerializeField] LayerMask hitMask; // set in Inspector


public ElementType Element { get; private set; }
GameObject owner;
float spawnTime;
Rigidbody2D rb;


void Awake() { rb = GetComponent<Rigidbody2D>(); }


public void Init(GameObject owner, ElementType elementType)
{
this.owner = owner; Element = elementType; spawnTime = Time.time;
rb.linearVelocity = transform.up * speed;
}


void Update()
{
if (Time.time - spawnTime >= lifetime) Destroy(gameObject);
}


void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log($"Projectile hit {other.name} on layer {LayerMask.LayerToName(other.gameObject.layer)}");

    if (other.attachedRigidbody && other.attachedRigidbody.gameObject == owner)
        return;

    if (((1 << other.gameObject.layer) & hitMask.value) == 0)
        return;

    if (other.TryGetComponent(out Health hp))
    {
        Debug.Log($"Damaging {other.name}");
        hp.TakeDamage(baseDamage);
    }

    Destroy(gameObject);
}


}