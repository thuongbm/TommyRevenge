using UnityEngine;

/// <summary>
/// When the player dies the GameObject stays ACTIVE (so the death animation / dead pose can be seen).
/// We only cut every control: no more hits, movement or shooting.
/// The lose panel appears afterwards (LoseScreenManager).
/// </summary>
public class PlayerDead : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private bool handled;

    void Update()
    {
        if (handled) return;
        if (BodyMovement.Instance == null || !BodyMovement.Instance.isDead) return;
        handled = true;

        if (boxCollider2D != null) boxCollider2D.enabled = false;   // nothing can hit the body anymore
        if (playerMovement != null) playerMovement.enabled = false; // can't walk

        var shooting = GetComponent<PlayerShooting>();
        if (shooting != null) shooting.enabled = false;              // can't shoot

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
