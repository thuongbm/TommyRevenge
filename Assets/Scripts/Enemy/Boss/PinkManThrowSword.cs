using UnityEngine;

public class PinkManThrowSword : MonoBehaviour
{
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject swordPrefab;

    [Header("Skill Settings")]
    public float skillCoolDown = 8f;
    [Tooltip("Boss bắt đầu ném kiếm khi người chơi trong tầm này.")]
    [SerializeField] private float maxThrowRange = 10f;
    [Tooltip("Boss sẽ không ném kiếm nếu người chơi quá gần (để tránh spam ở cận chiến).")]
    [SerializeField] private float minThrowRange = 2f;

    private float skillCounter;
    private Transform player;
    private FieldOfView2D fov;

    // True khi cooldown kết thúc và Boss đang trong điều kiện sẵn sàng ném.
    public bool IsThrowing { get; private set; }

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // Bắt đầu với cooldown đầy để Boss không ném ngay khi vào scene.
        skillCounter = skillCoolDown;
    }

    void Update()
    {
        if (skillCounter > 0f)
        {
            skillCounter -= Time.deltaTime;
        }

        // IsThrowing = true chỉ khi: cooldown hết + thấy player + player còn sống + đúng tầm.
        IsThrowing = skillCounter <= 0f && CanThrow();
    }

    private bool CanThrow()
    {
        if (player == null) return false;
        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead) return false;
        if (fov != null && !fov.canSeePlayer) return false;

        float dist = Vector2.Distance(transform.position, player.position);
        return dist >= minThrowRange && dist <= maxThrowRange;
    }

    // Được gọi bởi Animation Event trên clip bossThrowSword tại frame phóng kiếm.
    public void ThrowSword()
    {
        if (skillCounter > 0f) return;
        if (swordPrefab == null || throwPoint == null) return;
        if (!CanThrow()) return;

        // Tính hướng từ throwPoint tới player, quay sprite kiếm theo đúng hướng bay.
        Vector2 direction = ((Vector2)player.position - (Vector2)throwPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject sword = Instantiate(swordPrefab, throwPoint.position, rotation);

        // Truyền hướng bay vào SwordManager để kiếm đi đúng hướng.
        SwordManager sm = sword.GetComponent<SwordManager>();
        if (sm != null) sm.SetDirection(direction);

        skillCounter = skillCoolDown;
        IsThrowing = false;
    }
}
