using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Water;

public class MOBFOV : MonoBehaviour
{
    public float viewRange = 20.0f;
    [Range(0, 360)]
    public float viewAngle = 120.0f;

    private Transform MOBTr;
    private Transform playerTr;

    private int playerLayer;
    private int barrelLayer;
    private int layerMask;

    private readonly string playerTag = "Player";
    private readonly string playerLay = "PLAYER";
    private readonly string barrelLay = "BARREL";
    void Start()
    {
        MOBTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag(playerTag).GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer(playerLay);
        barrelLayer = LayerMask.NameToLayer(barrelLay);
        layerMask = 1 << playerLayer | 1 << barrelLayer;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] cols = Physics.OverlapSphere(MOBTr.position, viewRange, 1 << playerLayer);
        if (cols.Length == 1)
        {
            Vector3 dir = (playerTr.position - MOBTr.position).normalized;
            if (Vector3.Angle(MOBTr.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }
    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - MOBTr.position).normalized;

        if (Physics.Raycast(MOBTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag(playerTag));
        }

        return isView;
    }
}
