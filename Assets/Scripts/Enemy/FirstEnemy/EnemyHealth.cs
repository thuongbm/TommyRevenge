using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyState enemyState;
    [SerializeField] EnemyShooting enemyShooting;
    [SerializeField] MeleeAttack meleeAttack;


    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] FieldOfView2D fieldOfView2D;
    private Rigidbody2D rb;
    private NavMeshAgent navMeshAgent;

    public bool isDead;

    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead)
        {
            StopAllMovement();
            enabled = false; // no need to keep running Update
        }
    }

    private void StopAllMovement()
    {
        // Disable AI and combat scripts
        if (enemyState != null) enemyState.enabled = false;
        if (boxCollider2D != null) boxCollider2D.enabled = false;
        if (fieldOfView2D != null) fieldOfView2D.enabled = false;
        if (enemyShooting != null) enemyShooting.enabled = false;
        if (meleeAttack != null) meleeAttack.enabled = false;

        // Stop NavMeshAgent FIRST (it overwrites Rigidbody velocity each frame)
        if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.enabled = false;
        }

        // Then stop Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
