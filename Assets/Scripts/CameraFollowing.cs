using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Follow & Smoothing")]
    [SerializeField] private float smoothTime = 0.12f;
    [SerializeField] private float cameraZ = -10f;

    [Header("Mouse Lead (Lookahead)")]
    [Range(0.1f, 0.9f)]
    [SerializeField] private float leadWeight = 0.35f;      
    [SerializeField] private float maxLeadDistance = 4.5f;   

    [Header("Scout / Shift Look")]
    [SerializeField] private KeyCode scoutKey = KeyCode.LeftShift;
    [SerializeField] private float scoutLeadWeight = 0.7f;   
    [SerializeField] private float maxScoutDistance = 9f;    

    private Vector3 currentVelocity = Vector3.zero;
    private Camera cam;

    [Header("Screen Shake")]
    [SerializeField] private float comboShakeMagnitude = 0.15f;
    [SerializeField] private float comboShakeDuration = 0.2f;
    [SerializeField] private float winShakeMagnitude = 0.35f;
    [SerializeField] private float winShakeDuration = 0.5f;
    private float shakeTimer = 0f;
    private float shakeMagnitude = 0f;


private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        ComboManager.OnComboIncreased += HandleComboIncreased;
        LevelManager.OnLevelWon += HandleLevelWon;
    }

    private void OnDisable()
    {
        ComboManager.OnComboIncreased -= HandleComboIncreased;
        LevelManager.OnLevelWon -= HandleLevelWon;
    }

    private void HandleComboIncreased(int multiplier)
    {
        if (multiplier >= 2)
        {
            Shake(comboShakeMagnitude, comboShakeDuration);
        }
    }

    private void HandleLevelWon()
    {
        Shake(winShakeMagnitude, winShakeDuration);
    }

    public void Shake(float magnitude, float duration)
    {
        shakeMagnitude = magnitude;
        shakeTimer = duration;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -cameraZ; // Distance from camera to 2D plane (z = 0)
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        Vector3 toMouse = mouseWorldPos - player.position;
        toMouse.z = 0f;

        bool isScouting = Input.GetKey(scoutKey);
        float currentWeight = isScouting ? scoutLeadWeight : leadWeight;
        float currentMaxDist = isScouting ? maxScoutDistance : maxLeadDistance;

        Vector3 leadOffset = Vector3.ClampMagnitude(toMouse * currentWeight, currentMaxDist);
        Vector3 targetPosition = player.position + leadOffset;
        targetPosition.z = cameraZ;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);

        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            Vector3 shakeOffset = (Vector3)UnityEngine.Random.insideUnitCircle * shakeMagnitude;
            transform.position += shakeOffset;
        }
    }
}
