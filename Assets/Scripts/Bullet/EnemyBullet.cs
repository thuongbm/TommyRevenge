using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f;

    [Header("Blood Splash Sound")]
    public AudioClip bloodSplashSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            BodyMovement.Instance.isDead = true;
            BloodPlayerManage.Instance.BloodSpalsh();
            Debug.Log("Died");
            Debug.Log("Hit: " + collision.gameObject.name);
        }

        if (collision.gameObject.CompareTag("map"))
        {
            Destroy(gameObject);
        }

        audioSource.PlayOneShot(bloodSplashSound);
    }
}