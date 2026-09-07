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

    private void Awake()
    {
        cam = GetComponent<Camera>();
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
    }
}
