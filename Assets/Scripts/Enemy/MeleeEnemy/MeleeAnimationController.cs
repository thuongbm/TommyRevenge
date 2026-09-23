using System.Runtime.CompilerServices;
using UnityEngine;

public class MeleeAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private EnemyHealth enemyHealth;
    private EnemyState enemyState;
    private MeleeAttack meleeAttack;
    private FieldOfView2D fov;

    private float shootEffectCounter;

    private static readonly int IsDieHash = Animator.StringToHash("isDie");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsFiringHash = Animator.StringToHash("isFiring");

    void Awake()
    {
        if (enemyAnimator == null) 
            enemyAnimator = GetComponentInChildren<Animator>();

        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = GetComponent<EnemyState>();
        meleeAttack = GetComponent<MeleeAttack>();
        fov = GetComponent<FieldOfView2D>();
    }

    void Update()
    {
        if (enemyAnimator == null) return;

        if (enemyHealth != null && enemyHealth.isDead)
        {
            enemyAnimator.SetBool(IsDieHash, true);
            return;
        }

        if (enemyState != null)
        {
            enemyAnimator.SetBool(IsRunningHash, !enemyState.isWaiting);
        }

        shootEffectCounter += Time.deltaTime;

        bool canSee = fov != null && fov.canSeePlayer;

        if (canSee)
        {
            enemyAnimator.SetBool(IsRunningHash, true);
        }

        if (meleeAttack != null)
        {
            enemyAnimator.SetBool(IsFiringHash, meleeAttack.isAttack);
        }
    }
}
