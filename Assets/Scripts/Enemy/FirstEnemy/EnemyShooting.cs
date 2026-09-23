using System;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Casing")]
    [SerializeField] private GameObject bulletCasingPrefab;
    [SerializeField] private Transform dropBulletCasingPoint;
    [SerializeField] private float casingLifetime = 3f;
    [SerializeField] private float timeBetweenShoot = 0.5f; // Set to realistic interval (0.001f is too fast)

    [Header("Fire sound")]
    [SerializeField] private AudioClip fireClip;

    private AudioSource audioSource;
    private float fireCoolDown;
    private FieldOfView2D fov;

    // Event invoked precisely when a shot occurs
    public event Action onShoot;

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead) return;

        if (fireCoolDown > 0f)
        {
            fireCoolDown -= Time.deltaTime;
        }

        bool canSee = fov != null && fov.canSeePlayer;

        if (canSee && fireCoolDown <= 0f)
        {
            Shoot();
            fireCoolDown = timeBetweenShoot;
        }
    }

    public void Shoot()
    {
        // 1. Notify animation controller
        onShoot?.Invoke();

        // 2. Spawn bullet
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        // 3. Spawn casing
        if (bulletCasingPrefab != null && dropBulletCasingPoint != null)
        {
            GameObject casing = Instantiate(
                bulletCasingPrefab, 
                dropBulletCasingPoint.position, 
                dropBulletCasingPoint.rotation
            );

            Destroy(casing, casingLifetime);
        }

        // 4. Play audio
        if (audioSource != null && fireClip != null)
        {
            audioSource.PlayOneShot(fireClip);
        }
    }
}