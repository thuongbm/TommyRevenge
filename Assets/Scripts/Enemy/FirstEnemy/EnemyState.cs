using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum AIState { Patrol, Alerted, Chasing }

    [Header("Guard Type")]
    [SerializeField] private bool isStaticGuard = false;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoint;
    [SerializeField] private float waitTimePoint = 1.5f;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float alertSpeed = 5f; 
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float stopDistance = 0.2f;

    [Header("References")]
    [SerializeField] private Transform playerTransform;

    // [Header("Follow Player time")]
    // [SerializeField] float followPlayerTime = 2f;
    private float followPlayerCooldown;

    private AIState currentState = AIState.Patrol;
    private Vector3 soundTargetPos;

    private Transform currentPatrolPoint;
    private int currentPatrolIndex;
    private float waitTimer;
    public bool isWaiting;

    private FieldOfView2D fov;

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();
    }

    void Start()
    {
        if (patrolPoint != null && patrolPoint.Length > 0)
        {
            currentPatrolIndex = 0;
            currentPatrolPoint = patrolPoint[currentPatrolIndex];
        }
    }

    void Update()
    {
        followPlayerCooldown = Time.deltaTime;
        if (fov != null && fov.canSeePlayer && playerTransform != null)
        {
            currentState = AIState.Chasing;
            RotateTowards(playerTransform.position);
            return;
        }

        if (currentState == AIState.Alerted)
        {
            MoveTowardsTarget(soundTargetPos, alertSpeed);

            if (Vector2.Distance(transform.position, soundTargetPos) <= stopDistance)
            {
                currentState = AIState.Patrol;
            }
            return;
        }

        HandlePatrol();
    }


    public void MoveTowardsTarget(Vector3 targetPosition, float currentSpeed)
    {
        Vector3 target = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        RotateTowards(target);
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        followPlayerCooldown = 0f;

    }
    public void OnHeardGunShot(Vector2 soundPosition)
    {
        if (isStaticGuard) return;

        soundTargetPos = soundPosition;
        currentState = AIState.Alerted;
        isWaiting = false;
    }
    private void HandlePatrol()
    {
        if (currentPatrolPoint == null) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoint.Length;
                currentPatrolPoint = patrolPoint[currentPatrolIndex];
            }
            return;
        }

        MoveTowardsTarget(currentPatrolPoint.position, speed);

        if (Vector2.Distance(transform.position, currentPatrolPoint.position) <= stopDistance)
        {
            isWaiting = true;
            waitTimer = waitTimePoint;
        }
    }
}