#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView2D))]
public class FieldOfView2DEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView2D fov = (FieldOfView2D)target;

        Vector3 eye = fov.EyePosition;
        float facing = fov.transform.eulerAngles.z + fov.viewDirectionOffset;

        Handles.color = Color.white;
        Handles.DrawWireArc(eye, Vector3.forward, Vector3.up, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(facing, -fov.angle / 2f);
        Vector3 viewAngle02 = DirectionFromAngle(facing, fov.angle / 2f);

        Handles.color = Color.yellow;
        Handles.DrawLine(eye, eye + viewAngle01 * fov.radius);
        Handles.DrawLine(eye, eye + viewAngle02 * fov.radius);

        Handles.color = Color.cyan;
        Handles.DrawSolidDisc(eye, Vector3.forward, 0.06f);

        if (fov.canSeePlayer && fov.playerRef != null)
        {
            Handles.color = Color.green;
            Handles.DrawLine(eye, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerZ, float angleInDegrees)
    {
        angleInDegrees += eulerZ;

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
#endif
