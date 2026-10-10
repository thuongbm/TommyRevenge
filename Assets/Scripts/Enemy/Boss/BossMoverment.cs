using UnityEngine;
using UnityEngine.AI;

public class BossMoverment : MonoBehaviour
{
    private enum BossState { Idle, Chasing, Searching }

    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [Tooltip("The boss stops this far from the player instead of walking into him.")]
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Behaviour")]
    [Tooltip("Seconds the boss keeps hunting the last place he saw the player after losing sight.")]
    [SerializeField] private float searchTime = 4f;
    [SerializeField] private float repathInterval = 0.15f;

    [Header("References (auto-filled when left empty)")]
    [SerializeField] private Transform playerPosition;
    [SerializeField] private FieldOfView2D fieldOfView;
    [SerializeField] private Animator animator;

    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

    private NavMeshAgent agent;
    private Rigidbody2D rb;

    private BossState state = BossState.Idle;
    private Vector2 lastKnownPlayerPos;
    private Vector2 moveDirection;
    private bool isMoving;
    private float searchTimer;
    private float repathTimer;

    // True while the boss can see the player and has closed in to stopDistance.
    // Hook your attack script up to this.
    public bool IsPlayerInRange { get; private set; }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();

        if (animator == null) animator = GetComponent<Animator>();
        if (fieldOfView == null) fieldOfView = GetComponent<FieldOfView2D>();

        if (playerPosition == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerPosition = player.transform;
        }

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = speed;
            agent.stoppingDistance = stopDistance;

            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                Debug.LogWarning("BossMoverment: set the Rigidbody2D to Kinematic when using a NavMeshAgent, otherwise they fight each other.", this);
            }
        }
        else if (rb != null)
        {
            // Rotation is driven by this script, so physics must not spin the boss when he bumps into things.
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (playerPosition == null) return;

        if (BodyMovement.Instance != null && BodyMovement.Instance.isDead)
        {
            state = BossState.Idle;
            IsPlayerInRange = false;
            Stop();
            SetRunningAnimation();
            return;
        }

        bool canSee = fieldOfView != null && fieldOfView.canSeePlayer;
        Vector2 position = transform.position;
        Vector2 playerPos = playerPosition.position;

        if (canSee)
        {
            state = BossState.Chasing;
            lastKnownPlayerPos = playerPos;
            searchTimer = searchTime;
        }
        else if (state == BossState.Chasing)
        {
            state = BossState.Searching;
        }

        switch (state)
        {
            case BossState.Chasing:
                MoveTo(playerPos);
                break;

            case BossState.Searching:
                searchTimer -= Time.deltaTime;
                if (searchTimer <= 0f)
                {
                    state = BossState.Idle;
                    Stop();
                }
                else
                {
                    MoveTo(lastKnownPlayerPos);
                }
                break;

            default:
                Stop();
                break;
        }

        IsPlayerInRange = canSee && Vector2.Distance(position, playerPos) <= stopDistance + 0.25f;

        // Keep the view cone on the player while he is visible, otherwise look where we are walking.
        Vector2 faceDirection = canSee ? playerPos - position : moveDirection;
        FaceDirection(faceDirection);

        SetRunningAnimation();
    }

    private void MoveTo(Vector2 target)
    {
        Vector2 position = transform.position;

        if (Vector2.Distance(position, target) <= stopDistance)
        {
            Stop();
            return;
        }

        isMoving = true;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = speed;

            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                agent.SetDestination(target);
                repathTimer = repathInterval;
            }

            moveDirection = agent.velocity;
        }
        else
        {
            moveDirection = (target - position).normalized;

            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                rb.linearVelocity = moveDirection * speed;
            }
            else
            {
                transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
            }
        }
    }

    private void Stop()
    {
        isMoving = false;
        moveDirection = Vector2.zero;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
        }
        else if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void FaceDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.0001f) return;

        // Same offset the FieldOfView2D uses, so the cone and the sprite always agree on "forward".
        float facingOffset = fieldOfView != null ? fieldOfView.viewDirectionOffset : 0f;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - facingOffset;
        float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    private void SetRunningAnimation()
    {
        if (animator != null && animator.isActiveAndEnabled)
        {
            animator.SetBool(IsRunningHash, isMoving);
        }
    }

    // When a health/death script disables this component the boss halts instead of coasting on leftover velocity.
    void OnDisable()
    {
        Stop();
        SetRunningAnimation();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
