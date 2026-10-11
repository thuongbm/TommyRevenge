using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private BossAttacking bossAttacking;
    private PinkmanRoundAndRound pinkmanRoundAndRound;

    private static readonly int IsFiringHash = Animator.StringToHash("isFiring");
    private static readonly int IsSkillEHash = Animator.StringToHash("isSkillE");

    void Awake()
    {
        if (enemyAnimator == null) enemyAnimator = GetComponent<Animator>();

        bossAttacking = GetComponent<BossAttacking>();
        pinkmanRoundAndRound = GetComponent<PinkmanRoundAndRound>();
    }

    void Update()
    {
        if (enemyAnimator == null) return;

        // Both parameters are Bools, so mirror the boss's state every frame instead of firing triggers.
        bool spinning = pinkmanRoundAndRound != null && pinkmanRoundAndRound.IsSpinning;
        enemyAnimator.SetBool(IsSkillEHash, spinning);

        // The spin is his attack while it lasts, so don't also swing.
        bool attacking = bossAttacking != null && bossAttacking.isAttack && !spinning;
        enemyAnimator.SetBool(IsFiringHash, attacking);
    }
}
