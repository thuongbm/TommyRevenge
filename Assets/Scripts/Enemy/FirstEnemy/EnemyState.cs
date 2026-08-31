using System.Collections;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public static EnemyState Instance { get; private set; }

    [SerializeField] private Transform[] patrolPoint;
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float waitTimePoint = 1.5f;

    private Transform currentPatrolPoint;
    private int currentPatrolIndex;
    private float waitTimer;
    public bool isWaiting;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
        if (FieldOfView2D.Instance != null && FieldOfView2D.Instance.canSeePlayer)
        {
            if (playerTransform != null)
            {
                RotateTowards(playerTransform.position);
            }

            
            return; 
        }

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

        Vector3 targetPos = new Vector3(currentPatrolPoint.position.x, currentPatrolPoint.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        RotateTowards(targetPos);

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            isWaiting = true;
            waitTimer = waitTimePoint;
        }
    }

    private void RotateTowards(Vector3 targetPosition)
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
    }
}