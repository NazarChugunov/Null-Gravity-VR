using UnityEngine;

// Дає об'єкту легкий початковий дрейф і повільне обертання у невагомості,
// щоб предмети повільно плавали станцією, а не висіли мертвим грузом.
[RequireComponent(typeof(Rigidbody))]
public class GentleDrift : MonoBehaviour
{
    [Tooltip("Максимальна початкова лінійна швидкість (м/с)")]
    public float maxLinearSpeed = 0.12f;
    [Tooltip("Максимальна початкова кутова швидкість (рад/с)")]
    public float maxAngularSpeed = 0.25f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null || rb.isKinematic) return;

        // Випадковий повільний дрейф у будь-якому напрямку
        rb.linearVelocity = Random.onUnitSphere * Random.Range(maxLinearSpeed * 0.4f, maxLinearSpeed);
        // М'яке обертання навколо випадкової осі
        rb.angularVelocity = Random.onUnitSphere * Random.Range(maxAngularSpeed * 0.3f, maxAngularSpeed);
    }
}
