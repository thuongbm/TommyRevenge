using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private BossAttacking bossAttacking;
    private PinkmanRoundAndRound pinkmanRoundAndRound;
    private BossMoverment bossMoverment;

    private static readonly int IsFiringHash  = Animator.StringToHash("isFiring");
    private static readonly int IsSkillEHash  = Animator.StringToHash("isSkillE");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

    void Awake()
    {
        if (enemyAnimator == null) enemyAnimator = GetComponent<Animator>();

        bossAttacking        = GetComponent<BossAttacking>();
        pinkmanRoundAndRound = GetComponent<PinkmanRoundAndRound>();
        bossMoverment        = GetComponent<BossMoverment>();
    }

    void Update()
    {
        if (enemyAnimator == null) return;

        bool spinning = pinkmanRoundAndRound != null && pinkmanRoundAndRound.IsSpinning;

        // Skill E spin animation: takes priority over everything while active.
        enemyAnimator.SetBool(IsSkillEHash, spinning);

        // While spinning, suppress the running animation so only the spin plays.
        // The boss still physically moves via NavMesh/Rigidbody — animation only.
        if (spinning)
        {
            enemyAnimator.SetBool(IsRunningHash, false);
        }

        // The spin is his attack while it lasts, so don't also play the swing animation.
        bool attacking = bossAttacking != null && bossAttacking.isAttack && !spinning;
        enemyAnimator.SetBool(IsFiringHash, attacking);
    }
}

