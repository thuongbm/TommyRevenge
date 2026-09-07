using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.isDead = true;
            }

            BloodManage bloodManage = collision.gameObject.GetComponent<BloodManage>();
            if (bloodManage != null)
            {
                bloodManage.BloodSpalsh();
            }

            Debug.Log("Enemy Died");
            Debug.Log("Hit: " + collision.gameObject.name);
        }

        if (collision.gameObject.CompareTag("map"))
        {
            Destroy(gameObject);
        }
    }
}