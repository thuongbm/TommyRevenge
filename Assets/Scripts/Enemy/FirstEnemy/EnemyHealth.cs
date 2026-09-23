using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyState enemyState;
    [SerializeField] EnemyShooting enemyShooting;
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
            if (enemyShooting != null) enemyShooting.enabled = false;
            if (meleeAttack != null) meleeAttack.enabled = false;
        }
    }
}