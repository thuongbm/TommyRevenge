using UnityEngine;

public class BodyAnimationController : MonoBehaviour
{
    [SerializeField] Animator bodyAnimator;
    private bool isDead;

    void Start()
    {
        isDead = false;
    }

    void Update()
    {
       bodyAnimator.SetBool("isDead", false);
    }
}
