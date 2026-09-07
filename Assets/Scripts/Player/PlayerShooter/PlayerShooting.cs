using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevelPhysics2D;

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

    [Header("Audio Setting")]
    [SerializeField] private float soundRadius = 15f;
    [SerializeField] private LayerMask enemyLayer;


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

        AlertEnemies(transform.position, soundRadius);
    }

    private void AlertEnemies(Vector2 origin, float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, enemyLayer);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<EnemyState>(out EnemyState enemy))
            {
                enemy.OnHeardGunShot(origin);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }
}