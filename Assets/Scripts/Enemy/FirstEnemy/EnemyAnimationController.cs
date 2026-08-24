using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;

    void Update()
    {
        if (EnemyState.Instance.isWaiting)
        {
            enemyAnimator.SetBool("isRunning", false);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", true);
        }

        if (EnemyHealth.Instance.isDieing)
        {
            enemyAnimator.SetBool("isDie", true);
        }
    }
}
