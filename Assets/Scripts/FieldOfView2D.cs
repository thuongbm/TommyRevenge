using System.Collections;
using UnityEngine;

public class FieldOfView2D : MonoBehaviour
{
    public float radius = 5f;
    [Range(0, 360)]
    public float angle = 90f;

    [Header("Eye Setup")]
    [Tooltip("Where the view cone starts, in the object's local space. Example: (0, 0.45) puts it at the head of a sprite that faces up.")]
    public Vector2 eyeOffset = Vector2.zero;
    [Tooltip("Degrees added to the object's right axis to get the facing direction. 0 = faces right, 90 = faces up, -90 = faces down, 180 = faces left.")]
    public float viewDirectionOffset = 0f;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    public Vector2 EyePosition => transform.TransformPoint(eyeOffset);
    public Vector2 ViewDirection => Quaternion.Euler(0f, 0f, viewDirectionOffset) * transform.right;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Vector2 eye = EyePosition;
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(eye, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = ((Vector2)target.position - eye).normalized;

            if (Vector2.Angle(ViewDirection, directionToTarget) < angle / 2f)
            {
                float distanceToTarget = Vector2.Distance(eye, target.position);

                RaycastHit2D hit = Physics2D.Raycast(eye, directionToTarget, distanceToTarget, obstructionMask);

                if (hit.collider == null)
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}