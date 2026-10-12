using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] private float slowMultiplier = 0.5f; 
    
    [SerializeField] private float slowDuration = 2f; 

    [Header("Sword Stop Settings")]
    [SerializeField] private float stopTime = 2f;

    private bool isStopped = false;
    private Collider2D swordCollider;

    void Start()
    {
        swordCollider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!isStopped)
        {
           transform.Translate(Vector2.right * speed * Time.deltaTime); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Instance.ApplySlowDown(slowMultiplier, slowDuration);

            StartCoroutine(StopAndDespawnRoutine());
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