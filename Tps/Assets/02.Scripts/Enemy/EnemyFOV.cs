using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15.0f; //��ĳ������ ���� �����Ÿ�
    [Range(0, 360)] //�Ʒ��� ������ ������ 0~360���� �����Ѵ�.
    public float viewAngle = 120f; //��ĳ������ �þ߰�

    private Transform enemyTr;
    private Transform playerTr;

    private int playerLayer;
    private int boxLayer;
    private int barrelLayer;
    private int layerMask;

    private readonly string playerTag = "Player";
    private readonly string playerLay = "PLAYER";
    private readonly string boxLay = "BOXES";
    private readonly string barrelLay = "BARREL";
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag(playerTag).GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer(playerLay);
        boxLayer = LayerMask.NameToLayer(boxLay);
        barrelLayer = LayerMask.NameToLayer(barrelLay);
        layerMask = 1 << playerLayer | 1 << boxLayer | 1 << barrelLayer;
    }
    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y; //���� ��ǥ�� �������� �����ϱ� ���� ��ĳ������ Yȸ������ ���Ѵ�.
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
        //�������� 1�� ���� ���̶� �����ϸ� �ش� ����3�� x���� Sin = x/1, z���� cos = z/1�̴�.
        //Deg2Rad�� ���� ������ ��ȯ���ִ� ���. (PI*2)/360 �� �ǹ�. (Rad2Deg�� ������ �Ϲ� ������ ��ȯ�ϴ� ���)
    }

    public bool isTracePlayer() //�������� �Ǵ��ϴ� �Լ�.
    {
        bool isTrace = false;
        //���ʹ��� �����ǿ��� �÷��̾ 15�ݰ�ȿ� �÷��̾ �ִ��� �˻��ϰ� ������ cols�� ��´�.
        Collider[] cols = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
        if (cols.Length == 1) //�÷��̾ �����ȿ� �ִٸ�
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized; //�÷��̾ �ٶ󺸴� ������ ������.
            if (Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f)
            {//���ʹ̰� �ٶ󺸰��ִ� ���⿡�� �÷��̾ �ٶ󺸴� ������ �þ߰��� ������ �þ߰��� ���ݺ��� ������
                isTrace = true;//player�� �ٶ󺸰� �ִٰ� �Ǵ��Ͽ� �÷��̾ �Ѵ´�.
            }
        }
        return isTrace;
    }
    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - enemyTr.position).normalized; //�÷��̾ �ٶ󺸴� ����

        if(Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
        {//���ʹ��� ��ġ���� �÷��̾� �������� ������ ���� viewRange�ȿ��� �÷��̾�, ��ֹ�, �跲���� �¾�����
            isView = (hit.collider.CompareTag(playerTag)); //���� ��ü�� �ݶ��̴��� �±װ� �÷��̾�� true
        }
        return isView;
    }
}
