using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyState enemyState;
    [SerializeField] EnemyShooting enemyShooting;
    [SerializeField] BoxCollider2D boxCollider2D;
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
            enemyShooting.enabled = false;
        }
    }
}