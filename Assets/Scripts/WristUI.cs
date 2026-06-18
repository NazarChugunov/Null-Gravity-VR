using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WristUI : MonoBehaviour
{
    [Header("Підключення гравця")]
    public PlayerResources player; 

    [Header("Інтерфейс Здоров'я")]
    public Slider healthSlider;
    public TMP_Text healthPercentText;

    [Header("Інтерфейс Палива")]
    public Slider fuelSlider;
    public TMP_Text fuelPercentText;

    private void OnEnable()
    {
        // Підписуємося на події гравця, коли екран вмикається
        if (player != null)
        {
            player.OnHealthChanged.AddListener(UpdateHealthUI);
            player.OnFuelChanged.AddListener(UpdateFuelUI);
        }
    }

    private void OnDisable()
    {
        // Відписуємося, коли екран вимикається (для економії пам'яті)
        if (player != null)
        {
            player.OnHealthChanged.RemoveListener(UpdateHealthUI);
            player.OnFuelChanged.RemoveListener(UpdateFuelUI);
        }
    }

    // Ці методи автоматично спрацьовують ТІЛЬКИ тоді, коли гравець отримує шкоду або витрачає паливо
    private void UpdateHealthUI(float current, float max)
    {
        if (healthSlider != null)
            healthSlider.value = current / max;

        if (healthPercentText != null)
            healthPercentText.text = Mathf.RoundToInt((current / max) * 100).ToString() + "%";
    }

    private void UpdateFuelUI(float current, float max)
    {
        if (fuelSlider != null)
            fuelSlider.value = current / max;

        if (fuelPercentText != null)
            fuelPercentText.text = Mathf.RoundToInt((current / max) * 100).ToString() + "%";
    }
}