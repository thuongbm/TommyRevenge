using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; set;}
    [Header("Player setting")]
    [SerializeField] private float speed = 5f;
    private float originalSpeed; 
    private Coroutine slowCoroutine; 
    public Vector2 movementInput;
    private Rigidbody2D rb;
    [Header("Map")]
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (BodyMovement.Instance.isDead) return;
        
        PlayerMaxSpeed();
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void PlayerMaxSpeed()
    {
        rb.linearVelocity = movementInput * speed; 
    }

    public void ApplySlowDown(float speedMultiplier, float duration)
    {
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        
        slowCoroutine = StartCoroutine(SlowRoutine(speedMultiplier, duration));
    }

    private IEnumerator SlowRoutine(float multiplier, float duration)
    {
        speed = originalSpeed * multiplier; 
        
        yield return new WaitForSeconds(duration); 
        
        speed = originalSpeed; 
    }

}