using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 25f;
    public float lifeTime = 5f; // Через скільки секунд снаряд зникне, щоб не засмічувати пам'ять

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Відключаємо гравітацію для снаряда про всяк випадок
        rb.useGravity = false;

        // Запускаємо снаряд чітко вперед
        rb.linearVelocity = transform.forward * speed;

        // Таймер самознищення
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Шукаємо інтерфейс IDamageable на об'єкті, в який влучили
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        // Тут у майбутньому ми додамо створення візуального ефекту вибуху (іскор)

        // Знищуємо сам снаряд після влучання
        Destroy(gameObject);
    }
}