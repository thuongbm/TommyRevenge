using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] float timeBetweenAttack = 0.001f;
    [SerializeField] float attackCoolDown;
    [SerializeField] FieldOfView2D fov;

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();
    }

    void Update()
    {
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }

        if (fov != null && fov.canSeePlayer && attackCoolDown <= 0f)
        {
            while (attackCoolDown <= 0)
            {
                Attack();
                attackCoolDown += timeBetweenAttack;
            }
        }
    }
    private void Attack()
    {
        
    }
}
