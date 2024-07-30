using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCast : MonoBehaviour
{
    private Transform tr;
    private Ray ray; // ����
    private RaycastHit hit; // �������� ����ü : ������Ʈ�� ������ �¾Ҵ��� �Ǻ��Ѵ�.
    private float dist = 20f;
    public CrossHair Chr;
    
    void Start()
    {
        tr = GetComponent<Transform>();
        Chr = GameObject.Find("Canvas-UI").transform.GetChild(3).GetComponent<CrossHair>();
        // ���̶�Ű �ȿ��ִ� Canvas-UI�� ã�� �� ������Ʈ�� ���� �ε����� 3�� �ڽ� ������Ʈ�� ã�´�. �� ������Ʈ�� CrossHair������Ʈ�� ��´�.
    }



    void Update()
    {
        Mob_search();
    }

    private void Mob_search()
    {
        ray = new Ray(tr.position, tr.forward); //���� �Ҵ� ���ڸ��� �߻� ��ġ�� ����, �Ÿ��� ��������.
        //            �߻� ��ġ , ���� forward
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.green);//��ȭ�鿡�� ������ ���̰� �׽�Ʈ������ �����.
        //            �߻� ��ġ,  ����   * �Ÿ�(20) ,  ���̴� �÷�   (����� �Ÿ��� ���ϸ� velocity�� �ȴ�.)
        if (Physics.Raycast(ray, out hit, dist, 1 << 6 | 1 << 7 | 1 << 8)) //1<<6�� ��Ʈ �����ڸ� ����Ͽ� �Ǻ� ����. (6,7,8���̾� ��� ����)
        {//������ ����� �� ���� �¾Ҵ��� (������ ������ ���� ���� ������, �浹 ������Ʈ ��ġ ��, ���� ����, �� �Ǻ�)
            Chr.isGaze = true;
        }
        else // ������ ���� �ʾҴٸ�
        {
            Chr.isGaze = false;
        }
    }
}
