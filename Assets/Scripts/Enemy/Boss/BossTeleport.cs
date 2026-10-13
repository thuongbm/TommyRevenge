using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossTeleport : MonoBehaviour
{
    [SerializeField] private SwordManager swordManager;
    [SerializeField] private Animator bossAnimator;

    [Header("Teleport")]
    [Tooltip("Seconds after the sword sticks before the boss starts teleporting.")]
    [SerializeField] private float teleportDelay = 0.6f;
    [Tooltip("How long the boss is hidden mid-blink (plays appear VFX at the end).")]
    [SerializeField] private float travelDuration = 0.4f;
    [Tooltip("How long after appearing the boss stays in 'teleporting' state (immune, animation).")]
    [SerializeField] private float appearDuration = 0.3f;
    [Tooltip("Teleport automatically once the sword has stopped. Turn off to call TriggerTeleport() yourself.")]
    [SerializeField] private bool autoTeleport = true;

    [Header("VFX")]
    [Tooltip("Animation clip biến mất của Boss (bossTeleport.anim). Code tự đọc độ dài để chờ đúng.")]
    [SerializeField] private AnimationClip vanishClip;
    [Tooltip("Prefab spawn tại vị trí kiếm khi Boss xuất hiện (e.g. Explorsion).")]
    [SerializeField] private GameObject appearVFXPrefab;
    [Tooltip("Seconds before the appear VFX is destroyed (match the explosion animation length).")]
    [SerializeField] private float vfxLifetime = 0.5f;

    private static readonly int IsTeleportHash = Animator.StringToHash("isTeleport");

    private PinkManThrowSword thrower;
    private BossMoverment mover;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private Collider2D bossCollider;
    private SpriteRenderer[] spriteRenderers;

    private Coroutine teleportCoroutine;

    // Vị trí kiếm đã dừng — cache ngay khi nhận event, trước khi sword bị Destroy.
    private Vector2? pendingDestination;

    // True while the boss is teleporting: immune and can't be interrupted.
    public bool IsTeleporting { get; private set; }

    void Awake()
    {
        thrower         = GetComponent<PinkManThrowSword>();
        mover           = GetComponent<BossMoverment>();
        agent           = GetComponent<NavMeshAgent>();
        rb              = GetComponent<Rigidbody2D>();
        bossCollider    = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (bossAnimator == null) bossAnimator = GetComponent<Animator>();
    }

    // ── Được SwordManager gọi ngay khi kiếm dừng lại ──────────────────────────
    public void OnSwordStopped(Vector2 swordPosition)
    {
        if (IsTeleporting) return;
        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead) return;
        if (!autoTeleport) return;

        // Cache vị trí ngay — sword sẽ bị Destroy sau stopTime, nên không thể đọc sau.
        pendingDestination = swordPosition;

        if (teleportCoroutine != null) StopCoroutine(teleportCoroutine);
        teleportCoroutine = StartCoroutine(TeleportAfterDelay(swordPosition));
    }

    // Safe to call manually from an animation event or another AI script.
    public void TriggerTeleport()
    {
        if (IsTeleporting) return;
        if (!pendingDestination.HasValue) return;
        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead) return;

        if (teleportCoroutine != null) StopCoroutine(teleportCoroutine);
        teleportCoroutine = StartCoroutine(TeleportSequence(pendingDestination.Value));
        pendingDestination = null;
    }

    private IEnumerator TeleportAfterDelay(Vector2 destination)
    {
        yield return new WaitForSeconds(teleportDelay);

        if (IsTeleporting) yield break;
        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead) yield break;

        pendingDestination = null;
        teleportCoroutine = StartCoroutine(TeleportSequence(destination));
    }

    private IEnumerator TeleportSequence(Vector2 destination)
    {
        // --- PHASE 1: Phát animation biến mất ---
        IsTeleporting = true;
        if (bossCollider != null) bossCollider.enabled = false;
        if (bossAnimator != null) bossAnimator.SetBool(IsTeleportHash, true);

        // Chờ đúng bằng độ dài clip (fallback = travelDuration nếu clip chưa gán)
        float vanishWait = (vanishClip != null) ? vanishClip.length : travelDuration;
        yield return new WaitForSeconds(vanishWait);

        // --- PHASE 2: Blink tới vị trí kiếm ---
        SetVisible(false);
        BlinkTo(destination);
        SetVisible(true);

        // --- PHASE 3: Xuất hiện + VFX ---
        if (appearVFXPrefab != null)
        {
            GameObject vfx = Instantiate(appearVFXPrefab, destination, Quaternion.identity);
            Destroy(vfx, vfxLifetime);
        }

        yield return new WaitForSeconds(appearDuration);

        FinishTeleport();
    }

    private void BlinkTo(Vector2 destination)
    {
        if (mover != null) mover.Stop();

        if (agent != null && agent.isOnNavMesh)
        {
            agent.Warp(destination);
        }
        else
        {
            transform.position = new Vector3(destination.x, destination.y, transform.position.z);

            if (rb != null)
            {
                rb.position = destination;
                rb.linearVelocity = Vector2.zero;
            }
        }

        Physics2D.SyncTransforms();
    }

    private void SetVisible(bool visible)
    {
        foreach (var sr in spriteRenderers)
            if (sr != null) sr.enabled = visible;
    }

    private void FinishTeleport()
    {
        IsTeleporting = false;
        if (bossCollider != null) bossCollider.enabled = true;
        if (bossAnimator  != null) bossAnimator.SetBool(IsTeleportHash, false);
        teleportCoroutine = null;
    }

    // If a death script switches this component off mid-blink, restore the boss properly.
    void OnDisable()
    {
        if (!IsTeleporting && teleportCoroutine == null) return;

        if (teleportCoroutine != null)
        {
            StopCoroutine(teleportCoroutine);
            teleportCoroutine = null;
        }
        SetVisible(true);
        if (IsTeleporting) FinishTeleport();
    }
}
