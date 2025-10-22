using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHP = 100f;
    float currentHP;

    void Awake() => currentHP = maxHP;

    public void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0f)
            Die();
    }

    void Die() => Destroy(gameObject);
}
