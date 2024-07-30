using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class swatFOV : MonoBehaviour
{
    public float viewRange = 15.0f;
    [Range(0, 360)]
    public float viewAngle = 120.0f;

    private Transform swatTr;
    private Transform playerTr;

    private int playerLayer;
    private int barrelLayer;
    private int boxLayer;
    private int layerMask;

    private readonly string playerTag = "Player";
    private readonly string playerLay = "PLAYER";
    private readonly string barrelLay = "BARREL";
    private readonly string boxLay = "BOXES";
    void Start()
    {
        swatTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag(playerTag).GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer(playerLay);
        barrelLayer = LayerMask.NameToLayer(barrelLay);
        boxLayer = LayerMask.NameToLayer(boxLay);
        layerMask = 1 << playerLayer | 1 << barrelLayer | 1 << boxLayer;
    }
    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] cols = Physics.OverlapSphere(swatTr.position, viewRange, 1 << playerLayer);
        if (cols.Length == 1)
        {
            Vector3 dir = (playerTr.position - swatTr.position).normalized;
            if(Vector3.Angle(swatTr.forward, dir) < viewAngle * 0.5f)
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
        Vector3 dir = (playerTr.position - swatTr.position).normalized;

        if(Physics.Raycast(swatTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag(playerTag));
        }

        return isView;
    }
}
