using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private BossAttacking bossAttacking;

    private static readonly int IsFiringHash = Animator.StringToHash("isFiring");

    void Awake()
    {
        bossAttacking = GetComponent<BossAttacking>();
    }

    void Update()
    {
        if (bossAttacking.isAttack && bossAttacking != null)
        {
            enemyAnimator.SetTrigger(IsFiringHash);
        }
    }
}
