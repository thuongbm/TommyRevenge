using UnityEngine;
using UnityEngine.InputSystem;

public class BodyMovement : MonoBehaviour
{
    public static BodyMovement Instance { get; set; }
    private Camera mainCamera;

    private float nextFireTime;
    public bool isDead;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
        isDead = false;
    }

    void Update()
    {
        if (isDead) return;
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
}