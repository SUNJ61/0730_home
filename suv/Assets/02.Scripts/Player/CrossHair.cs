using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    Transform tr; //지정하지 않아도 private로 저장
    public Image crossHair;

    float startTime; //크로스헤어가 커지기 시작한 시간을 저장
    float duration = 0.25f; //크로스헤어가 커지는 속도 
    float minSize = 0.6f; //크로스헤어 최소크기
    float maxSize = 1.2f; //크로스헤어 최대크기
    public bool isGaze = false; // 응시중인지 확인하는 변수

    Color originColor = new Color(1f, 1f, 1f, 0.8f); //색지정, 흰색, 투명도 0.8, 크로스헤어 초기색
    Color gazeColor = Color.red; // 적 응시중일 때 크로스헤어 빨간색으로 지정
    void Start()
    {
        tr = transform;
        crossHair = GetComponent<Image>();
        startTime = Time.time;
        tr.localScale = Vector3.one * minSize; //초기 크기
        //localScale사용 이유 -> 부모의 속한 자식클래스인 크로스헤어가 독자적인 크기를 가지기 위해, Vector3.one은 xyz동일한 크기를 가지기 위해 
        crossHair.color = originColor;
    }

    
    void Update()
    {
        CrossHair_ctrl();
    }

    private void CrossHair_ctrl()
    {
        if (isGaze) //광선이 적을 주시중이라면
        {
            float time = (Time.time - startTime) / duration; // time  = 지난시간 / 0.25
            tr.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, time);//Mathf - 수학관련 기능/lerp(초기크기,변할크기,걸리는시간)
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
