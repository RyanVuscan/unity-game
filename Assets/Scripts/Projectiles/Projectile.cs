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
Rigidbody rb;


void Awake() { rb = GetComponent<Rigidbody>(); }


public void Init(GameObject owner, ElementType elementType)
{
this.owner = owner; Element = elementType; spawnTime = Time.time;
rb.linearVelocity = transform.forward * speed;
}


void Update()
{
if (Time.time - spawnTime >= lifetime) Destroy(gameObject);
}


void OnTriggerEnter(Collider other)
{
    if (other.attachedRigidbody && other.attachedRigidbody.gameObject == owner)
        return;

    if (((1 << other.gameObject.layer) & hitMask.value) == 0)
        return;

    if (other.TryGetComponent(out Health hp))
    {
        float damage = baseDamage;
        
        // Check for elemental weakness/strength
        if (other.TryGetComponent(out ElementalAffinity affinity))
        {
            damage *= ElementRules.DamageMultiplier(Element, affinity.Element);
        }
        
        hp.TakeDamage(damage);
    }

    Destroy(gameObject);
}


}