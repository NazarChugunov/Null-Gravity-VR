using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RepairTool : MonoBehaviour
{
    [Header("Налаштування вібрації")]
    [Range(0f, 1f)] public float vibrationIntensity = 0.5f; // Поставив 0.5 для чіткого відчуття
    public float vibrationDuration = 0.05f;

    [Header("Зв'язок з руків'ям")]
    [Tooltip("Перетягни сюди головний об'єкт RepairTool (на якому висить XR Grab Interactable)")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    [Header("Візуальний ефект ремонту")]
    [Tooltip("Партикл-система, що грає, поки інструмент ремонтує модуль")]
    public ParticleSystem repairEffect;

    [Header("Звук ремонту")]
    [Tooltip("AudioSource (loop), що звучить під час ремонту")]
    public AudioSource repairAudio;

    void Start()
    {
        // Підстраховка: якщо забули перетягнути вручну, скрипт знайде його сам
        if (grabInteractable == null)
        {
            grabInteractable = GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        RepairableModule module = other.GetComponent<RepairableModule>();

        if (module != null)
        {
            // Якщо ремонт іде (не дійшов до 100%)
            if (module.RepairTick(Time.deltaTime))
            {
                // Якщо ми зараз тримаємо інструмент у руці
                if (grabInteractable != null && grabInteractable.isSelected)
                {
                    // Сучасний синтаксис (той самий, що на слотах)
                    var hand = grabInteractable.interactorsSelecting[0];

                    if (hand is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
                    {
                        // Прямий імпульс у контролер
                        controllerInteractor.SendHapticImpulse(vibrationIntensity, vibrationDuration);
                    }

                    // Вмикаємо візуальний ефект ремонту
                    SetEffect(true);
                }
                else
                {
                    SetEffect(false);
                }
            }
            else
            {
                // Ремонт завершено або не активний — вимикаємо ефект
                SetEffect(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Коли наконечник відведений від модуля — гасимо ефект
        if (other.GetComponent<RepairableModule>() != null)
        {
            SetEffect(false);
        }
    }

    private void SetEffect(bool on)
    {
        if (on)
        {
            if (repairEffect != null && !repairEffect.isEmitting) repairEffect.Play();
            if (repairAudio != null && !repairAudio.isPlaying) repairAudio.Play();
        }
        else
        {
            if (repairEffect != null && repairEffect.isEmitting)
                repairEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if (repairAudio != null && repairAudio.isPlaying)
                repairAudio.Stop();
        }
    }
}