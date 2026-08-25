using NUnit.Framework;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance { get; set; }
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public bool isDieing;

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
        currentHealth = maxHealth;
        isDieing = false;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            isDieing = true;
        }
    }
}
