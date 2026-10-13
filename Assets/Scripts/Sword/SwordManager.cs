using UnityEngine;

public class SwordManager : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    [SerializeField] private float slowMultiplier = 0.5f;

    [SerializeField] private float slowDuration = 2f;

    [Header("Sword Stop Settings")]
    [Tooltip("How long the sword stays where it landed after touching the player.")]
    [SerializeField] private float stopTime = 2f;

    [Header("Auto Despawn")]
    [Tooltip("A sword that is still flying after this many seconds disappears.")]
    [SerializeField] private float lifetime = 6f;

    // True once the sword has touched the player and is staying where it landed.
    public bool isStopped = false;
    private Collider2D swordCollider;
    [SerializeField] private Animator swordAnimator;

    private SpriteRenderer spriteRenderer;
    private Vector2 flyDirection = Vector2.right;
    private float flyTimer;
    private float stopTimer;

    void Start()
    {
        swordCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f) Destroy(gameObject);
            return;
        }

        transform.Translate(flyDirection * speed * Time.deltaTime, Space.World);

        flyTimer += Time.deltaTime;
        if (flyTimer >= lifetime) Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        flyDirection = direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStopped) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.ApplySlowDown(slowMultiplier, slowDuration);
            }

            StopHere();
        }
        if (collision.gameObject.CompareTag("map"))
        {
            StopHere();
        }
    }

    // The sword stays exactly where it touched the player.
    private void StopHere()
    {
        isStopped = true;
        stopTimer = stopTime;

        if (swordCollider == null) swordCollider = GetComponent<Collider2D>();
        if (swordCollider != null) swordCollider.enabled = false;

        if (swordAnimator != null) swordAnimator.SetBool("isTouched", true);

        // Thông báo vị trí cho BossTeleport ngay lập tức (trước khi sword bị Destroy).
        BossTeleport bossTeleport = FindFirstObjectByType<BossTeleport>();
        if (bossTeleport != null) bossTeleport.OnSwordStopped(SwordLocation());
    }

    // Where the sword visually sits (centre of its sprite), so the boss lands on the sword and not on its pivot.
    public Vector2 SwordLocation()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        return spriteRenderer != null ? (Vector2)spriteRenderer.bounds.center : (Vector2)transform.position;
    }

    // The boss teleported to the sword, so it is used up.
    public void Consume()
    {
        Destroy(gameObject);
    }
}
