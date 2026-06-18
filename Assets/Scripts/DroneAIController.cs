using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneAIController : MonoBehaviour
{
    [Header("Налаштування цілі")]
    public Transform playerTarget;

    [Header("Налаштування польоту")]
    public float flySpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float attackDistance = 5.0f;
    public float visionDistance = 15.0f;
    public float detectionDistance = 10.0f; // Дистанція виявлення гравця

    [Header("Стрільба")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2.0f;

    [Header("Звук")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 0.5f;

    private float nextFireTime = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (playerTarget == null && Camera.main != null)
        {
            playerTarget = Camera.main.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTarget == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        
        // 1. Перевірка на виявлення гравця
        if (distanceToPlayer > detectionDistance)
        {
            StopFlying();
            return;
        }

        Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

        // Дрон завжди дивиться на гравця, якщо виявив його
        RotateTowards(directionToPlayer);

        // 2. Логіка руху (Прямий політ у 3D просторі)
        if (distanceToPlayer > attackDistance)
        {
            FlyTowards(directionToPlayer);
        }
        else
        {
            StopFlying();
        }

        // 3. Логіка стрільби (тільки якщо є пряма видимість)
        if (distanceToPlayer <= visionDistance)
        {
            if (HasLineOfSight())
            {
                Shoot();
            }
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed));
    }

    private void FlyTowards(Vector3 direction)
    {
        Vector3 newPosition = rb.position + direction * flySpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void StopFlying()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private bool HasLineOfSight()
    {
        if (playerTarget == null || firePoint == null) return false;

        Vector3 direction = (playerTarget.position - firePoint.position).normalized;

        // Промінь від точки стрільби до гравця
        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, visionDistance))
        {
            // Якщо першим об'єктом є гравець (або частина його ієрархії)
            if (hit.transform.root == playerTarget.root || hit.transform == playerTarget)
            {
                return true;
            }
        }
        return false;
    }

    private void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            if (projectilePrefab != null && firePoint != null)
            {
                Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                if (shootSound != null)
                    AudioSource.PlayClipAtPoint(shootSound, firePoint.position, shootVolume);
            }
            nextFireTime = Time.time + fireRate;
        }
    }
}
