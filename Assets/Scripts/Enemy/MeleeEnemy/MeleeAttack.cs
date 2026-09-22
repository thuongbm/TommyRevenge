using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float timeBetweenAttack = 1f;
    [SerializeField] private Transform playerTransform;

    private float attackCoolDown;
    private FieldOfView2D fov;

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();
    }

    void Update()
    {
        if (attackCoolDown > 0)
            attackCoolDown -= Time.deltaTime;

        
    }

    private bool InRange()
    {
        return playerTransform != null &&
               Vector2.Distance(transform.position, playerTransform.position) <= attackRange;
    }

    private void Attack()
    {
        Debug.Log("Attacked");
        BodyMovement.Instance.isDead = true;
        BloodPlayerManage.Instance.BloodSpalsh();
    }
}