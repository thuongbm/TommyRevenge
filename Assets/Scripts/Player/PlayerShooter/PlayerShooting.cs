using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting Instance { get; private set; }

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private readonly float[] spreadAngles = { -12f, 0f, 12f };

    [Header("Casing")]
    [SerializeField] private GameObject bulletCasingPrefab;
    [SerializeField] private Transform dropBulletCasingPoint;
    [SerializeField] private float casingLifetime = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        foreach (float angle in spreadAngles)
        {
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            Instantiate(bulletPrefab, firePoint.position, rotation);
        }

        if (bulletCasingPrefab != null && dropBulletCasingPoint != null)
        {
            GameObject casing = Instantiate(
                bulletCasingPrefab, 
                dropBulletCasingPoint.position, 
                dropBulletCasingPoint.rotation
            );

            Destroy(casing, casingLifetime);
        }
    }
}