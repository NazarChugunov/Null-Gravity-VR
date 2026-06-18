using UnityEngine;

// Автоматичні двері: коли гравець наближається (входить у тригер),
// вмикається параметр аніматора "character_nearby" -> двері роз'їжджаються.
// Коли гравець відходить -> двері зачиняються.
// Нічого з логіки гри не змінює, вішається окремо на двері.
public class DoorProximity : MonoBehaviour
{
    [Tooltip("Радіус, у якому гравець відчиняє двері (метри)")]
    public float detectionRadius = 3.5f;

    [Tooltip("Ім'я bool-параметра в аніматорі дверей")]
    public string animatorBool = "character_nearby";

    private Animator[] animators;
    private int nearbyCount = 0;

    void Awake()
    {
        // Двері паку мають аніматор на корені та на стулках — керуємо всіма.
        animators = GetComponentsInChildren<Animator>(true);
    }

    private void SetNearby(bool value)
    {
        if (animators == null) return;
        foreach (var a in animators)
            if (a != null) a.SetBool(animatorBool, value);
    }

    private bool IsPlayer(Collider other)
    {
        // Гравець — це XR Origin, на якому висить PlayerResources.
        return other.GetComponentInParent<PlayerResources>() != null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            nearbyCount++;
            SetNearby(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            nearbyCount = Mathf.Max(0, nearbyCount - 1);
            if (nearbyCount == 0) SetNearby(false);
        }
    }
}
