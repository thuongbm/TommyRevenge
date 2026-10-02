using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Update()
    {
        if (BodyMovement.Instance.isDead)
        {
            boxCollider2D.enabled = false;
            playerMovement.enabled = false;

            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }
}
