using UnityEngine;
using UnityEngine.Events; // ϳ�������� ������� ���� (Observer)

public class PlayerResources : MonoBehaviour, IDamageable
{
    [Header("������'�")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("������")]
    public float maxFuel = 100f;
    private float currentFuel;
    public float fuelConsumptionRate = 15f;

    [Header("Відновлення палива")]
    [Tooltip("Скільки секунд без використання джетпака до початку відновлення")]
    public float fuelRegenDelay = 10f;
    [Tooltip("Швидкість відновлення палива за секунду")]
    public float fuelRegenRate = 20f;
    private float regenTimer = 0f;

    [Header("��䳿 (Observer)")]
    // ֳ ��䳿 ��������� ��� �����: ������� �������� �� �����������
    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent<float, float> OnFuelChanged;

    void Start()
    {
        currentHealth = maxHealth;
        currentFuel = maxFuel;

        // ��������� ��������� �� ����� ���
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnFuelChanged?.Invoke(currentFuel, maxFuel);
    }

    // ����� ���������� IDamageable
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������ �� ��'������ ������'�

        Debug.Log($"<color=red>������� ������� �����!</color> ����������: {currentHealth}");

        // ����������� ���������, �� ������'� ��������
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ����� ��� ��������
    public bool TryConsumeFuel()
    {
        // Будь-яка спроба тяги скидає таймер відновлення
        regenTimer = 0f;

        if (currentFuel > 0)
        {
            currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

            // ����������� ���������, �� ������ ��������
            OnFuelChanged?.Invoke(currentFuel, maxFuel);
            return true;
        }

        return false;
    }

    // Викликається з джетпака при натисканні тяги (навіть коли пальне скінчилось),
    // щоб утримати таймер відновлення скинутим, поки гравець тримає тягу.
    public void NotifyThrustUsed()
    {
        regenTimer = 0f;
    }

    // Відновлення палива після паузи без використання джетпака
    void Update()
    {
        if (currentFuel < maxFuel)
        {
            regenTimer += Time.deltaTime;
            if (regenTimer >= fuelRegenDelay)
            {
                currentFuel += fuelRegenRate * Time.deltaTime;
                currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
                OnFuelChanged?.Invoke(currentFuel, maxFuel);
            }
        }
    }

    private void Die()
    {
        Debug.Log("������� �������! ���������� �������...");

        // ��������� ����� ������� � ������ GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}