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

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("map"))
        {
            Destroy(gameObject);
            Debug.Log("Hit: " + collision.gameObject.name);
        }
    }
}