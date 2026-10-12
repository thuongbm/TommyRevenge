using UnityEngine;

public class PinkmanRoundAndRound : MonoBehaviour
{
    public int skillIndex = 1;
    [Tooltip("Cooldown timer. Counts down after a spin ends; the skill is ready at 0.")]
    public float skillCounter;
    public float skillCooldown = 10f;
    public GameObject skillECircle;

    [Header("Spin")]
    [Tooltip("How long one spin lasts, in seconds.")]
    [SerializeField] private float spinDuration = 3f;
    [Tooltip("The boss runs this many times faster while spinning.")]
    [SerializeField] private float spinSpeedMultiplier = 1.6f;
    [Tooltip("The boss starts spinning when he can see the player and is closer than this.")]
    [SerializeField] private float triggerRange = 7f;
    [Tooltip("Distance from the boss to the player's centre that counts as a spin hit.")]
    [SerializeField] private float hitRadius = 1.8f;

    [Header("Attack sound")]
    [SerializeField] private AudioClip swordSound;

    private AudioSource audioSource;
    private FieldOfView2D fieldOfView;
    private Transform player;
    private float spinTimer;

    // True for the whole time the boss is doing his spin.
    public bool IsSpinning { get; private set; }
    // 1 normally, spinSpeedMultiplier while spinning (BossMoverment reads this).
    public float SpeedMultiplier => IsSpinning ? spinSpeedMultiplier : 1f;
    // True khi cooldown kết thúc và điều kiện bắt đầu chiêu đã thỏa mãn.
    // BossAnimationController đọc property này để quyết định có nên kích hoạt chiêu không.
    public bool IsReady => skillCounter <= 0f && !IsSpinning && CanStartSpin();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        fieldOfView = GetComponent<FieldOfView2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        if (skillECircle != null) skillECircle.SetActive(false);
    }

    void Update()
    {
        if (IsSpinning)
        {
            spinTimer -= Time.deltaTime;

            if (spinTimer <= 0f || IsPlayerDead())
            {
                EndSpin();
            }
            return;
        }

        if (skillCounter > 0f)
        {
            skillCounter -= Time.deltaTime;
        }

        // Việc quyết định BẮT ĐẦU chiêu được giao cho BossAnimationController
        // để đảm bảo thứ tự ưu tiên: ThrowSword > RoundAndRound > NormalAttack.
    }

    private bool CanStartSpin()
    {
        if (player == null || fieldOfView == null || !fieldOfView.canSeePlayer) return false;
        if (IsPlayerDead()) return false;

        return Vector2.Distance(transform.position, player.position) <= triggerRange;
    }

    private bool IsPlayerDead()
    {
        return BodyMovement.Instance != null && BodyMovement.Instance.isDead;
    }

    public void StartSpin()
    {
        IsSpinning = true;
        spinTimer = spinDuration;
    }

    private void EndSpin()
    {
        IsSpinning = false;
        skillCounter = skillCooldown;

        if (skillECircle != null) skillECircle.SetActive(false);
    }

    public void Passive()
    {

    }

    // Called by the animation events inside the bossRoundAndRound clip (once per spin hit).
    public void SkillEAttack()
    {
        if (!IsSpinning) return;

        if (skillECircle != null) skillECircle.SetActive(true);
        if (audioSource != null && swordSound != null) audioSource.PlayOneShot(swordSound);

        if (player == null || IsPlayerDead()) return;

        Vector2 center = skillECircle != null ? (Vector2)skillECircle.transform.position : (Vector2)transform.position;

        if (Vector2.Distance(center, player.position) <= hitRadius)
        {
            Debug.Log("Spin hit the player");

            if (BodyMovement.Instance != null) BodyMovement.Instance.isDead = true;
            if (BloodPlayerManage.Instance != null) BloodPlayerManage.Instance.BloodSpalsh();
        }
    }

    // If a death script disables this component mid-spin, don't leave the circle switched on.
    void OnDisable()
    {
        IsSpinning = false;
        if (skillECircle != null) skillECircle.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, hitRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
}
