using UnityEngine;

public class BloodManage : MonoBehaviour
{
    [SerializeField] private GameObject blood;
    private EnemyHealth enemyHealth;

    void Awake()
    {
        // Get the health component attached to this specific enemy
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void BloodSpalsh()
    {
        // Check this local enemy's health instead of the global Instance
        if (enemyHealth != null && enemyHealth.isDead)
        {
            GameObject bloodInstance = Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(bloodInstance, 1.0f);
        }
    }
}