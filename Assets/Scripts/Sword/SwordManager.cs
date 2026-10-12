using System.Collections;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    
    [SerializeField] private float slowMultiplier = 0.5f; 
    
    [SerializeField] private float slowDuration = 2f; 

    [Header("Sword Stop Settings")]
    [SerializeField] private float stopTime = 2f;

    [Header("Auto Despawn")]
    [Tooltip("Kiếm sẽ tự hủy sau thời gian này (giây) nếu không trúng gì.")]
    [SerializeField] private float lifetime = 6f;

    private bool isStopped = false;
    private Collider2D swordCollider;

    // Hướng bay (được gán bởi PinkManThrowSword sau khi Instantiate).
    private Vector2 flyDirection = Vector2.right;

    void Start()
    {
        swordCollider = GetComponent<Collider2D>();
        // Tự hủy sau lifetime giây để tránh rác khi bắn trượt.
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!isStopped)
        {
            // Dùng Space.World để kiếm bay theo hướng thế giới, không phụ thuộc góc xoay cục bộ.
            transform.Translate(flyDirection * speed * Time.deltaTime, Space.World);
        }
    }

    /// <summary>
    /// Được gọi bởi PinkManThrowSword ngay sau Instantiate để thiết lập hướng bay.
    /// </summary>
    public void SetDirection(Vector2 direction)
    {
        flyDirection = direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStopped) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // Chỉ làm chậm nếu player chưa chết.
            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.ApplySlowDown(slowMultiplier, slowDuration);
            }
            StartCoroutine(StopAndDespawnRoutine());
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            // Trúng vật cản (tường, sàn...) → hủy ngay.
            Destroy(gameObject);
        }
    }

    private IEnumerator StopAndDespawnRoutine()
    {
        isStopped = true; 
        
        if (swordCollider != null)
        {
            swordCollider.enabled = false; 
        }

        yield return new WaitForSeconds(stopTime);
        
        Destroy(gameObject); 
    }
}