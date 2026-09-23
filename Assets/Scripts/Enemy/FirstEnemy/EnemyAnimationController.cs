using System.Collections;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private float shootEffectDuration = 0.05f;

    private EnemyHealth enemyHealth;
    private EnemyState enemyState;
    private EnemyShooting enemyShooting;
    private FieldOfView2D fov;
    private Coroutine shootAnimCoroutine;

    private static readonly int IsDieHash = Animator.StringToHash("isDie");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsFiringHash = Animator.StringToHash("isFiring");

    void Awake()
    {
        if (enemyAnimator == null) 
            enemyAnimator = GetComponentInChildren<Animator>();

        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = GetComponent<EnemyState>();
        enemyShooting = GetComponent<EnemyShooting>();
        fov = GetComponent<FieldOfView2D>();
    }

    void OnEnable()
    {
        if (enemyShooting != null)
        {
            enemyShooting.onShoot += TriggerShootAnimation;
        }
    }

    void OnDisable()
    {
        if (enemyShooting != null)
        {
            enemyShooting.onShoot -= TriggerShootAnimation;
        }
    }

    void Update()
    {
        if (enemyAnimator == null) return;

        // Handle death
        if (enemyHealth != null && enemyHealth.isDead)
        {
            if (shootAnimCoroutine != null) StopCoroutine(shootAnimCoroutine);
            enemyAnimator.SetBool(IsFiringHash, false);
            enemyAnimator.SetBool(IsRunningHash, false);
            enemyAnimator.SetBool(IsDieHash, true);
            enabled = false;
            return;
        }

        // Handle running
        bool isMoving = (enemyState != null && !enemyState.isWaiting);
        if (fov != null && fov.canSeePlayer)
        {
            isMoving = true;
        }
        enemyAnimator.SetBool(IsRunningHash, isMoving);
    }

    private void TriggerShootAnimation()
    {
        if (shootAnimCoroutine != null)
        {
            StopCoroutine(shootAnimCoroutine);
        }
        shootAnimCoroutine = StartCoroutine(ShootPulseRoutine());
    }

    private IEnumerator ShootPulseRoutine()
    {
        enemyAnimator.SetBool(IsFiringHash, true);
        yield return new WaitForSeconds(shootEffectDuration);
        enemyAnimator.SetBool(IsFiringHash, false);
        shootAnimCoroutine = null;
    }
}