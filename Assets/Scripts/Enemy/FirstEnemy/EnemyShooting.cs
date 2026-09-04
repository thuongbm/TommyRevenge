using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Casing")]
    [SerializeField] private GameObject bulletCasingPrefab;
    [SerializeField] private Transform dropBulletCasingPoint;
    [SerializeField] private float casingLifetime = 3f;
    [SerializeField] private float timeBetweenShoot = 0.001f;

    private float fireCoolDown;
    private FieldOfView2D fov;

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();
    }

    void Update()
{
    if (BodyMovement.Instance.isDead) return;
    if (fireCoolDown > 0)
    {
        fireCoolDown -= Time.deltaTime;
    }

    if (fov != null && fov.canSeePlayer && fireCoolDown <= 0f)
    {
        while (fireCoolDown <= 0f)
        {
            Shoot();
            fireCoolDown += timeBetweenShoot; 
        }
    }
}

    public void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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