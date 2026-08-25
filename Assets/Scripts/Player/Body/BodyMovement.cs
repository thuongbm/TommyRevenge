using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    private Camera mainCamera;
    [Header("Weapon Stats")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.12f;
    [SerializeField] private float spreadAngle = 3.5f;
    [SerializeField] private bool isAutomatic = false;

    private float nextFireTime;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleAiming();
    }
    private void HandleAiming()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 lookDirection = mouseWorldPos - transform.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    //Bugs
    private void HandleShooting()
    {
        bool wantsToFire = isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

        if (wantsToFire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }
    }

    private void Fire()
    {
        // Calculate random spread
        float randomSpread = Random.Range(-spreadAngle, spreadAngle);
        Quaternion spreadRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + randomSpread);

        // Spawn bullet
        Instantiate(bulletPrefab, firePoint.position, spreadRotation);
    }
}