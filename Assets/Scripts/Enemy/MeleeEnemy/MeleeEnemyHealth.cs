using UnityEngine;

public class MeleeEnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyState enemyState;
    [SerializeField] MeleeAttack meleeAttack;

    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] FieldOfView2D fieldOfView2D;
    public bool isDead;

    void Start()
    {
        isDead = false;
    }

void Update()
    {
        if (isDead)
        {
            enemyState.enabled = false;
            boxCollider2D.enabled = false;
            fieldOfView2D.enabled = false;
            if (meleeAttack != null) meleeAttack.enabled = false;
        }
    }
}