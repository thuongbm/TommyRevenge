using UnityEngine;

public class BodyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private GameObject legs;
    [SerializeField] private Animator legsAnimator;

    private bool hasHandledDeath = false;

    void Update()
    {
        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead && !hasHandledDeath)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        hasHandledDeath = true;

        if (bodyAnimator != null)
        {
            bodyAnimator.SetBool("isDead", true);
        }

        if (legs != null)
        {
            Destroy(legs);
        }
    }
}