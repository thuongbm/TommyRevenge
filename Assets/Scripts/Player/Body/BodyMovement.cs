using UnityEngine;
using UnityEngine.InputSystem;

public class BodyMovement : MonoBehaviour
{
    public static BodyMovement Instance { get; set; }
    private Camera mainCamera;

    private float nextFireTime;
    [Header("Immortal Skill (press E)")]
    [SerializeField] private float immortalDuration = 0.2f;
    [SerializeField] private float cooldownDuration = 5f;

    public bool isImmortal { get; private set; }
    private float immortalTimer;

    /// <summary>1 = just activated, 0 = finished / inactive. Read by the HUD.</summary>
    public float ImmortalRemaining01 => isImmortal ? Mathf.Clamp01(immortalTimer / immortalDuration) : 0f;
    private float cooldownTimer;

    /// <summary>True when the skill can be used (not active and not cooling down).</summary>
    public bool IsReady => !isImmortal && cooldownTimer <= 0f;

    /// <summary>1 = just used, 0 = ready again. Read by the HUD.</summary>
    public float CooldownRemaining01 => cooldownDuration <= 0f ? 0f : Mathf.Clamp01(cooldownTimer / cooldownDuration);


    private bool _isDead;
    // Every damage source does `BodyMovement.Instance.isDead = true`,
    // so blocking it here makes the player immune to all of them.
    public bool isDead
    {
        get => _isDead;
        set
        {
            if (value && isImmortal) return;
            _isDead = value;
        }
    }

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
        // Cooldown tick
        if (cooldownTimer > 0f)
            cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);

        // Press E -> immortal for `immortalDuration` seconds, then wait for the cooldown
        if (!isDead && IsReady && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isImmortal = true;
            immortalTimer = immortalDuration;
            cooldownTimer = cooldownDuration;
        }

        if (isImmortal)
        {
            immortalTimer -= Time.deltaTime;
            if (immortalTimer <= 0f)
            {
                immortalTimer = 0f;
                isImmortal = false;
            }
        }

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