using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    private EnemyHealth enemyHealth;
    private EnemyState enemyState;

    void Awake()
    {
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

        if (FieldOfView2D.Instance.canSeePlayer)
        {
            enemyAnimator.SetBool("isFiring", true);
        }
    }
}