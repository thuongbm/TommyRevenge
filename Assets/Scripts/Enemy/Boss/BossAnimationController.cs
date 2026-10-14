using UnityEngine;

/// <summary>
/// Điều phối thứ tự ưu tiên kỹ năng của Boss và cập nhật Animator.
///
/// Thứ tự ưu tiên (cao → thấp):
///   1. Ném kiếm (ThrowSword)   – cooldown 8s
///   2. Xoay kiếm (RoundAndRound) – cooldown 10s
///   3. Đánh thường (Normal Attack)
///
/// Quy tắc:
///   - Nếu nhiều chiêu cùng sẵn sàng → chiêu ưu tiên cao hơn được chọn.
///   - Khi một chiêu đang DIỄN RA (đang xoay / animation đang phát) → giữ
///     cho chiêu đó hoàn thành, không chen ngang.
/// </summary>
public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private BossAttacking bossAttacking;
    private PinkmanRoundAndRound pinkmanRoundAndRound;
    private PinkManThrowSword pinkManThrowSword;
    private BossAirstrikeSkill bossAirstrikeSkill;
    private BossMoverment bossMoverment;

    private static readonly int IsFiringHash   = Animator.StringToHash("isFiring");
    private static readonly int IsSkillEHash   = Animator.StringToHash("isSkillE");
    private static readonly int IsRunningHash  = Animator.StringToHash("isRunning");
    private static readonly int IsThrowingHash = Animator.StringToHash("isThrowing");
    private static readonly int IsCallingHash = Animator.StringToHash("isCalling");

    private static readonly int IsTeleportHash = Animator.StringToHash("isTeleport");

    void Awake()
    {
        if (enemyAnimator == null) enemyAnimator = GetComponent<Animator>();

        bossAttacking = GetComponent<BossAttacking>();
        pinkmanRoundAndRound = GetComponent<PinkmanRoundAndRound>();
        pinkManThrowSword = GetComponent<PinkManThrowSword>();
        bossMoverment = GetComponent<BossMoverment>();
        bossAirstrikeSkill = GetComponent<BossAirstrikeSkill>();
    }

    void Update()
    {
        if (enemyAnimator == null) return;

        bool playerDead = BodyMovement.Instance != null && BodyMovement.Instance.isDead;
        if (playerDead)
        {
            SetAllSkillsOff();
            return;
        }

        bool spinning = pinkmanRoundAndRound != null && pinkmanRoundAndRound.IsSpinning;
        if (spinning)
        {
            enemyAnimator.SetBool(IsThrowingHash, false);
            enemyAnimator.SetBool(IsSkillEHash,   true);
            enemyAnimator.SetBool(IsRunningHash,  false);
            enemyAnimator.SetBool(IsFiringHash,   false);
            enemyAnimator.SetBool(IsCallingHash, false);
            return;
        }

        bool throwAnimPlaying = enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("bossThrowSword");
        if (throwAnimPlaying)
        {
            // Giữ flag ở false để AnyState transition không kéo sang chiêu khác.
            enemyAnimator.SetBool(IsThrowingHash, false);
            enemyAnimator.SetBool(IsSkillEHash,   false);
            enemyAnimator.SetBool(IsFiringHash,   false);
            enemyAnimator.SetBool(IsCallingHash, false);
            return;
        }

        // ─────────────────────────────────────────────────────────────
        // Từ đây: không chiêu nào đang chạy → xét CHỌN chiêu mới.
        // ─────────────────────────────────────────────────────────────

        bool throwReady = pinkManThrowSword    != null && pinkManThrowSword.IsThrowing;
        bool spinReady  = pinkmanRoundAndRound != null && pinkmanRoundAndRound.IsReady;
        bool callReady = bossAirstrikeSkill != null && (bossAirstrikeSkill.isCalling || bossAirstrikeSkill.IsReady);

        // ── PRIORITY 1: Ném kiếm ─────────────────────────────────────
        if (throwReady)
        {
            enemyAnimator.SetBool(IsThrowingHash, true);
            enemyAnimator.SetBool(IsSkillEHash,   false);
            enemyAnimator.SetBool(IsCallingHash, false);
            enemyAnimator.SetBool(IsFiringHash,   false);
            return;
        }
        enemyAnimator.SetBool(IsThrowingHash, false);
        
        if (callReady)
        {
            if (bossAirstrikeSkill.IsReady) bossAirstrikeSkill.StartCall();
            enemyAnimator.SetBool(IsCallingHash, true);
            enemyAnimator.SetBool(IsThrowingHash, false);
            enemyAnimator.SetBool(IsSkillEHash,   false);
            enemyAnimator.SetBool(IsFiringHash,   false);
            return;
        }
        enemyAnimator.SetBool(IsCallingHash, false);

        // ── PRIORITY 2: Xoay kiếm ────────────────────────────────────
        if (spinReady)
        {
            pinkmanRoundAndRound.StartSpin();   // IsSpinning sẽ = true từ frame này
            enemyAnimator.SetBool(IsSkillEHash,  true);
            enemyAnimator.SetBool(IsRunningHash, false);
            enemyAnimator.SetBool(IsFiringHash,  false);
            enemyAnimator.SetBool(IsCallingHash, false);
            return;
        }
        enemyAnimator.SetBool(IsSkillEHash, false);

        // ── PRIORITY 3: Đánh thường ───────────────────────────────────
        bool attacking = bossAttacking != null && bossAttacking.isAttack;
        enemyAnimator.SetBool(IsFiringHash, attacking);
    }

    // Tắt hết tất cả các flag skill (dùng khi boss/player chết).
    private void SetAllSkillsOff()
    {
        if (enemyAnimator == null || !enemyAnimator.isActiveAndEnabled) return;
        enemyAnimator.SetBool(IsThrowingHash, false);
        enemyAnimator.SetBool(IsSkillEHash,   false);
        enemyAnimator.SetBool(IsFiringHash,   false);
        enemyAnimator.SetBool(IsCallingHash, false);
    }
}
