using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MOBFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        MOBFOV fov = (MOBFOV)target;
        Vector3 AnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);

        Handles.color = new Color(1, 1, 1, 0.3f);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, AnglePos, fov.viewAngle, fov.viewRange);

        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewAngle.ToString());
    }
}
