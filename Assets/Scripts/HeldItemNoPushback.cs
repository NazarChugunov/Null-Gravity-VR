using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Поки гравець тримає предмет у руці, вимикаємо колізію між цим предметом
// і колайдером тіла гравця, щоб тіло не відштовхувало предмет (без тряски/боротьби фізики).
// Після відпускання — колізію повертаємо.
[RequireComponent(typeof(XRGrabInteractable))]
public class HeldItemNoPushback : MonoBehaviour
{
    [Tooltip("Шар тіла гравця, з яким треба ігнорувати колізію під час тримання")]
    public string playerLayerName = "Player";

    private XRGrabInteractable grab;
    private Collider[] itemColliders;
    private readonly List<Collider> ignoredPlayerColliders = new List<Collider>();
    private int playerLayer = -1;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        itemColliders = GetComponentsInChildren<Collider>(true);
        playerLayer = LayerMask.NameToLayer(playerLayerName);
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
        // Підстраховка: якщо вимкнули компонент під час тримання — повертаємо колізії
        RestoreCollisions();
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (playerLayer < 0) return;

        // Знаходимо корінь ієрархії інтерактора (XR Origin) і беремо колайдери тіла гравця
        Transform root = args.interactorObject.transform.root;
        var rootColliders = root.GetComponentsInChildren<Collider>(true);

        foreach (var pc in rootColliders)
        {
            if (pc == null || pc.isTrigger) continue;
            if (pc.gameObject.layer != playerLayer) continue;

            ignoredPlayerColliders.Add(pc);
            foreach (var ic in itemColliders)
            {
                if (ic != null && ic.enabled)
                    Physics.IgnoreCollision(ic, pc, true);
            }
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        RestoreCollisions();
    }

    private void RestoreCollisions()
    {
        foreach (var pc in ignoredPlayerColliders)
        {
            if (pc == null) continue;
            foreach (var ic in itemColliders)
            {
                if (ic != null)
                    Physics.IgnoreCollision(ic, pc, false);
            }
        }
        ignoredPlayerColliders.Clear();
    }
}
