using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PushPullLocomotion : MonoBehaviour
{
    [Header("Кнопки захвату (Grip)")]
    public InputActionProperty leftGripInput;
    public InputActionProperty rightGripInput;

    [Header("Контролери")]
    public Transform leftController;
    public Transform rightController;

    [Header("Налаштування фізики")]
    public float pushForce = 1.0f;
    public float grabRadius = 0.15f;       // Радіус для початку захвату (15 см)
    public float breakGrabRadius = 0.3f;   // Радіус для зриву захвату (30 см)

    private Rigidbody rb;

    private bool isLeftGrabbing = false;
    private Vector3 leftPreviousPosition;

    private bool isRightGrabbing = false;
    private Vector3 rightPreviousPosition;

    public bool IsGrabbing => isLeftGrabbing || isRightGrabbing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (leftGripInput.action != null) leftGripInput.action.Enable();
        if (rightGripInput.action != null) rightGripInput.action.Enable();
    }

    void FixedUpdate()
    {
        ProcessHand(leftGripInput, leftController, ref isLeftGrabbing, ref leftPreviousPosition);
        ProcessHand(rightGripInput, rightController, ref isRightGrabbing, ref rightPreviousPosition);
    }

    // Оновлений метод для перевірки контакту, який ігнорує гравця та предмети (зброю)
    private bool IsTouchingWall(Vector3 position, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        foreach (var hit in hitColliders)
        {
            // Отримуємо номер шару об'єкта, якого торкнулися
            int hitLayer = hit.gameObject.layer;

            // Якщо це не гравець (Player), не предмет (Items) і не тригер — значить це стіна
            if (hitLayer != LayerMask.NameToLayer("Player") &&
                hitLayer != LayerMask.NameToLayer("Items") &&
                !hit.isTrigger)
            {
                return true;
            }
        }
        return false;
    }

    private void ProcessHand(InputActionProperty gripInput, Transform controller, ref bool isGrabbing, ref Vector3 previousPos)
    {
        float gripValue = gripInput.action.ReadValue<float>();

        if (gripValue > 0.1f)
        {
            if (!isGrabbing)
            {
                // Початок захвату
                if (IsTouchingWall(controller.position, grabRadius))
                {
                    isGrabbing = true;
                    previousPos = controller.localPosition;
                }
            }
            else
            {
                // ПОСТІЙНА ПЕРЕВІРКА: чи не відлетіла рука занадто далеко від стіни?
                if (!IsTouchingWall(controller.position, breakGrabRadius))
                {
                    isGrabbing = false; // Автоматичний зрив захвату
                    return;
                }

                Vector3 deltaPosition = controller.localPosition - previousPos;
                Vector3 worldDelta = transform.TransformDirection(deltaPosition);

                rb.MovePosition(rb.position - worldDelta);
                previousPos = controller.localPosition;
            }
        }
        else if (isGrabbing)
        {
            // Плане відштовхування при відпусканні кнопки
            isGrabbing = false;

            Vector3 handVelocity = (controller.localPosition - previousPos) / Time.fixedDeltaTime;
            Vector3 worldVelocity = transform.TransformDirection(handVelocity);

            rb.linearVelocity = -worldVelocity * pushForce;
        }
    }
}