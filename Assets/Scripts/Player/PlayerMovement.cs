using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {get; set;}
    [Header("Player setting")]
    [SerializeField] private float speed = 5f;
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
        rb.linearVelocity = movementInput * speed; 
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}