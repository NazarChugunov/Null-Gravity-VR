using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class Weapon : MonoBehaviour
{
    [Header("�������")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;

    [Header("³����� � ����������")]
    public float recoilForce = 2.0f;
    public Rigidbody playerRigidbody;

    [Header("����")]
    public AudioClip shootSound;
    [Range(0f, 1f)] public float shootVolume = 0.7f;

    private float nextFireTime = 0f;
    private XRGrabInteractable grabInteractable;

    // ��������� ��� �������� ������� � ������� ����
    private bool shouldApplyRecoil = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.activated.AddListener(OnShoot);
    }

    void OnDisable()
    {
        grabInteractable.activated.RemoveListener(OnShoot);
    }

    private void OnShoot(ActivateEventArgs args)
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        // 1. ��������� ������ (������ ������ ������)
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }

        // ���� ��������
        if (shootSound != null)
        {
            Vector3 pos = firePoint != null ? firePoint.position : transform.position;
            AudioSource.PlayClipAtPoint(shootSound, pos, shootVolume);
        }

        // 2. ���� ������� ����������� ������ (��� �� �� ����������� ��!)
        if (playerRigidbody != null)
        {
            shouldApplyRecoil = true;
        }
    }

    // �������: ��� ����� (AddForce) �� ��������� Ҳ���� ���!
    void FixedUpdate()
    {
        if (shouldApplyRecoil)
        {
            // �������� ������
            playerRigidbody.AddForce(-firePoint.forward * recoilForce, ForceMode.Impulse);

            // ������� ���������, ��� �� ������� � ������������
            shouldApplyRecoil = false;
        }
    }
}