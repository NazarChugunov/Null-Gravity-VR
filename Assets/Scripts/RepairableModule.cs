using UnityEngine;
using TMPro;

public class RepairableModule : MonoBehaviour
{
    public enum ModuleType { Light, Ventilation, Gravity }

    [Header("Налаштування Модуля")]
    public ModuleType moduleType;
    public float integrity = 0f;
    public float repairSpeed = 25f;
    private bool isRepaired = false;

    [Header("Інтерфейс (UI)")]
    public TMP_Text terminalText;

    [Header("Посилання на системи")]
    public Light[] lightsToEnable;
    public AudioSource ventAudio;

    [Header("Переміщення (Тільки для Гравітації)")]
    public MonoBehaviour zeroGMovement;  // Сюди перетягнеш скрипт невагомості
    public MonoBehaviour groundMovement; // Сюди перетягнеш скрипт звичайної ходьби

    void Update()
    {
        if (isRepaired) return;

        if (integrity >= 100f)
        {
            integrity = 100f;
            CompleteRepair();
        }
    }

    // Змінено на bool: повертає true, якщо ремонт іде, і false, якщо вже полагодили
    public bool RepairTick(float deltaTime)
    {
        if (isRepaired) return false;

        integrity += repairSpeed * deltaTime;

        if (terminalText != null && moduleType != ModuleType.Ventilation)
        {
            terminalText.text = $"ЦІЛІСНІСТЬ: {Mathf.FloorToInt(integrity)}%";
        }

        return true;
    }

    private void CompleteRepair()
    {
        isRepaired = true;

        // --- ОСЬ ЦЕЙ РЯДОК БУВ ПРОПУЩЕНИЙ ---
        if (GameManager.Instance != null) GameManager.Instance.ModuleRepaired();
        // ------------------------------------

        switch (moduleType)
        {
            case ModuleType.Light:
                foreach (Light l in lightsToEnable) { if (l != null) l.enabled = true; }
                if (terminalText != null) terminalText.text = "СВІТЛО: АКТИВНО";
                break;

            case ModuleType.Ventilation:
                if (terminalText != null) terminalText.text = "ВЕНТИЛЯЦІЯ: НОРМА";
                if (ventAudio != null) ventAudio.Play();
                break;

            case ModuleType.Gravity:
                if (terminalText != null) terminalText.text = "ГРАВІТАЦІЯ: НОРМА";

                // Вмикаємо глобальну фізику
                Physics.gravity = new Vector3(0, -9.81f, 0);

                // --- ПЕРЕМИКАЄМО РУХ ---
                if (zeroGMovement != null) zeroGMovement.enabled = false;
                if (groundMovement != null) groundMovement.enabled = true;
                // -----------------------

                // Збираємо всі об'єкти і змушуємо їх впасти
                Rigidbody[] allRigidbodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
                foreach (Rigidbody rb in allRigidbodies)
                {
                    rb.useGravity = true;
                    if (rb.isKinematic) rb.isKinematic = false;
                    rb.WakeUp();
                }
                break;
        }
    }
}