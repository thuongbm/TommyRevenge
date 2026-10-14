using System;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 20;

    [Header("Hit Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashTime = 0.08f;

    [Header("Death")]
    [SerializeField] private float destroyDelay = 0f; // 0 = keep the corpse

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool isDead { get; private set; }

    // For a health bar later: OnHealthChanged(current, max)
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    private Animator animator;
    private Rigidbody2D rb;
    private Color originalColor = Color.white;

    private static readonly int IsDieHash      = Animator.StringToHash("isDie");
    private static readonly int IsFiringHash   = Animator.StringToHash("isFiring");
    private static readonly int IsSkillEHash   = Animator.StringToHash("isSkillE");
    private static readonly int IsRunningHash  = Animator.StringToHash("isRunning");
    private static readonly int IsThrowingHash = Animator.StringToHash("isThrowing");
    private static readonly int IsTeleportHash = Animator.StringToHash("isTeleport");
    private static readonly int IsCallingHash  = Animator.StringToHash("isCalling");

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) originalColor = spriteRenderer.color;
        CurrentHealth = maxHealth;
    }

    void Start()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0) Die();
        else if (spriteRenderer != null) StartCoroutine(HitFlash());
    }

    private System.Collections.IEnumerator HitFlash()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitFlashTime);
        if (!isDead) spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        isDead = true;
        StopAllCoroutines();
        if (spriteRenderer != null) spriteRenderer.color = originalColor;

        // Stop every boss behaviour
        DisableIfExists<BossAnimationController>();
        DisableIfExists<BossMoverment>();
        DisableIfExists<BossAttacking>();
        DisableIfExists<PinkmanRoundAndRound>();
        DisableIfExists<PinkManThrowSword>();
        DisableIfExists<BossTeleport>();
        DisableIfExists<BossAirstrikeSkill>();
        DisableIfExists<FieldOfView2D>();

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (animator != null)
        {
            animator.SetBool(IsFiringHash, false);
            animator.SetBool(IsSkillEHash, false);
            animator.SetBool(IsRunningHash, false);
            animator.SetBool(IsThrowingHash, false);
            animator.SetBool(IsTeleportHash, false);
            animator.SetBool(IsCallingHash, false);
            animator.SetBool(IsDieHash, true);
        }

        LevelManager.OnEnemyKilled(transform.position); // same call BulletManager uses
        OnDied?.Invoke();

        if (destroyDelay > 0f) Destroy(gameObject, destroyDelay);
    }

    private void DisableIfExists<T>() where T : Behaviour
    {
        var c = GetComponent<T>();
        if (c != null) c.enabled = false;
    }
}