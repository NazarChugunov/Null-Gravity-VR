using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class SocketHaptics : MonoBehaviour
{
    [Header("Налаштування вібрації")]
    [Range(0f, 1f)] public float vibrationIntensity = 0.8f;
    public float vibrationDuration = 0.2f;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
    }

    void OnEnable()
    {
        socket.hoverEntered.AddListener(OnHoverEntered);
    }

    void OnDisable()
    {
        socket.hoverEntered.RemoveListener(OnHoverEntered);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        // Безпечно приводимо тип до інтерфейсу захоплення (IXRSelectInteractable)
        if (args.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable)
        {
            // Перевіряємо, чи об'єкт зараз тримають і чи є активні контролери
            if (selectInteractable.isSelected && selectInteractable.interactorsSelecting.Count > 0)
            {
                var hand = selectInteractable.interactorsSelecting[0];

                // Якщо рука є контролером гравця — відправляємо імпульс
                if (hand is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
                {
                    controllerInteractor.SendHapticImpulse(vibrationIntensity, vibrationDuration);
                }
            }
        }
    }
}