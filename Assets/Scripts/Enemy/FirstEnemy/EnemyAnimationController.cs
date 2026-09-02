using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private EnemyHealth enemyHealth;
    private EnemyState enemyState;
    private FieldOfView2D fov;

    [SerializeField] private float shootEffectCoolDown = 0.05f;
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
        fov = GetComponent<FieldOfView2D>();
    }

    void Update()
    {
        if (enemyAnimator == null) return;

        if (enemyHealth != null && enemyHealth.isDieing)
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
            enemyAnimator.SetBool(IsRunningHash, false);
        }
    }
}