using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type { NORMAL, SPAWNPOINT, FIREFOS }
    public Type type;
    private Color _color;
    private string SpawnPointFile = "Enemy";
    void Start()
    { 
        type = Type.NORMAL;
    }
    private void OnDrawGizmos()
    {
        if (type == Type.NORMAL)
        {
            _color = Color.yellow;
            Gizmos.color = _color;

            Gizmos.DrawSphere(transform.position, 0.3f);
        }
        else if (type == Type.SPAWNPOINT)
        {
            _color = Color.green;
            Gizmos.color = _color;

            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, SpawnPointFile, true);
        }
        else if (type == Type.FIREFOS)
        {
            _color = Color.red;
            Gizmos.color = _color;

            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}
