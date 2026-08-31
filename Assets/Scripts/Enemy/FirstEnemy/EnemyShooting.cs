using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [Header("Casing")]
    [SerializeField] private GameObject bulletCasingPrefab;
    [SerializeField] private Transform dropBulletCasingPoint;
    [SerializeField] private float casingLifetime = 3f;
    [SerializeField] private float timeBetweenShoot = 1f;
    private float fireCoolDown;


    void Update()
    {
        if (fireCoolDown > 0)
        {
            fireCoolDown -= Time.deltaTime;
        }

        if (FieldOfView2D.Instance.canSeePlayer && fireCoolDown <= 0f)
        {
            Shoot();
            EnemyAnimationController.Instance.EnemyShootingAnimation(FieldOfView2D.Instance.canSeePlayer);
            fireCoolDown = timeBetweenShoot;
        }
    }
    //Bug
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
