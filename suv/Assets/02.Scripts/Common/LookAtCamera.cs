using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    /*
    1. 캔버스 UI가 카메라를 쳐다본다.
    */
    public Transform mainCam;
    public Transform tr;

    void Start()
    {
        mainCam = Camera.main.transform; //maincamera 태그를 가진 카메라를 찾아 위치값은 준다.
        tr = GetComponent<Transform>(); //자기자신의 위치값을 표현
    }

    void Update()
    {
        tr.LookAt(mainCam); //캔버스가 메인캠을 바라본다.
    }
}
