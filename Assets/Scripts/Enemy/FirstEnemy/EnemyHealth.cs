using NUnit.Framework;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance { get; set; }
    [SerializeField] EnemyState enemyState;
    [SerializeField] EnemyShooting enemyShooting;
    [SerializeField] BoxCollider2D boxCollider2D;
    public bool isDead;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
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
