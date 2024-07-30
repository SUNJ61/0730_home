using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    Transform tr; //�������� �ʾƵ� private�� ����
    public Image crossHair;

    float startTime; //ũ�ν��� Ŀ���� ������ �ð��� ����
    float duration = 0.25f; //ũ�ν��� Ŀ���� �ӵ� 
    float minSize = 0.6f; //ũ�ν���� �ּ�ũ��
    float maxSize = 1.2f; //ũ�ν���� �ִ�ũ��
    public bool isGaze = false; // ���������� Ȯ���ϴ� ����

    Color originColor = new Color(1f, 1f, 1f, 0.8f); //������, ���, ���� 0.8, ũ�ν���� �ʱ��
    Color gazeColor = Color.red; // �� �������� �� ũ�ν���� ���������� ����
    void Start()
    {
        tr = transform;
        crossHair = GetComponent<Image>();
        startTime = Time.time;
        tr.localScale = Vector3.one * minSize; //�ʱ� ũ��
        //localScale��� ���� -> �θ��� ���� �ڽ�Ŭ������ ũ�ν��� �������� ũ�⸦ ������ ����, Vector3.one�� xyz������ ũ�⸦ ������ ���� 
        crossHair.color = originColor;
    }

    
    void Update()
    {
        CrossHair_ctrl();
    }

    private void CrossHair_ctrl()
    {
        if (isGaze) //������ ���� �ֽ����̶��
        {
            float time = (Time.time - startTime) / duration; // time  = �����ð� / 0.25
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, time);//Mathf - ���а��� ���/lerp(�ʱ�ũ��,����ũ��,�ɸ��½ð�)
            crossHair.color = gazeColor;
        }
        else
        {
            tr.localScale = Vector3.one * minSize;
            crossHair.color = originColor;
            startTime = Time.time;
        }
    }
}
