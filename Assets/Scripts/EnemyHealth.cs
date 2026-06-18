using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Характеристики")]
    public float maxHealth = 50f;
    private float currentHealth;

    [Header("Ефекти")]
    public GameObject explosionPrefab; // Ось ця змінна створить нове поле в Unity!

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Створюємо ефект вибуху
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion, 2f); // Видаляємо частинки через 2 секунди
        }

        // Знищуємо дрона
        Destroy(gameObject);
    }
}