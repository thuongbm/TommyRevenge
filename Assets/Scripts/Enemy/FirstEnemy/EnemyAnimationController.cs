using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public static EnemyAnimationController Instance { get; set; }
    [SerializeField] private Animator enemyAnimator;
    private EnemyHealth enemyHealth;
    private EnemyState enemyState;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (enemyAnimator == null) enemyAnimator = GetComponentInChildren<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyState = GetComponent<EnemyState>();
    }

    void Update()
    {
        if (enemyState != null)
        {
            enemyAnimator.SetBool("isRunning", !enemyState.isWaiting);
        }

        if (enemyHealth != null && enemyHealth.isDieing)
        {
            enemyAnimator.SetBool("isDie", true);
        }
    }

    public void EnemyShootingAnimation(bool canSeePlayer)
    {
        if (canSeePlayer)
        {
            enemyAnimator.SetBool("isFiring", true);
        }
        else
        {
            enemyAnimator.SetBool("isFiring", false);
            enemyAnimator.SetBool("isRunning", !enemyState.isWaiting);
        }
    }
}