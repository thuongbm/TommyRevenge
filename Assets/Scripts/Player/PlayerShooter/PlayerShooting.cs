using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    // Spread angles on the 2D Z-axis
    private float[] spreadAngles = { -12f, 0f, 12f };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
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
    }
}