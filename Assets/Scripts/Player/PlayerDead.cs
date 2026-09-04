using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] PlayerMovement playerMovement;

    void Update()
    {
        if (BodyMovement.Instance.isDead)
        {
            boxCollider2D.enabled = false;
            playerMovement.enabled = false;
        }
    }
}
