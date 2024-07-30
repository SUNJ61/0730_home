using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCast : MonoBehaviour
{
    private Transform tr;
    private Ray ray; // 광선
    private RaycastHit hit; // 광선관련 구조체 : 오브젝트가 관선에 맞았는지 판별한다.
    private float dist = 20f;
    public CrossHair Chr;
    
    void Start()
    {
        tr = GetComponent<Transform>();
        Chr = GameObject.Find("Canvas-UI").transform.GetChild(3).GetComponent<CrossHair>();
        // 하이라키 안에있는 Canvas-UI를 찾고 그 오브젝트에 속한 인덱스가 3인 자식 오브젝트를 찾는다. 그 오브젝트의 CrossHair컴포넌트를 얻는다.
    }



    void Update()
    {
        Mob_search();
    }

    private void Mob_search()
    {
        ray = new Ray(tr.position, tr.forward); //동적 할당 하자마자 발사 위치와 방향, 거리를 지정했음.
        //            발사 위치 , 방향 forward
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.green);//씬화면에서 광선이 보이게 테스트용으로 디버깅.
        //            발사 위치,  방향   * 거리(20) ,  보이는 컬러   (방향과 거리를 곱하면 velocity가 된다.)
        if (Physics.Raycast(ray, out hit, dist, 1 << 6 | 1 << 7 | 1 << 8)) //1<<6은 비트 연산자를 사용하여 판별 가능. (6,7,8레이어 모두 감지)
        {//광선을 쏘았을 때 적이 맞았는지 (광선이 위에서 일정 범위 감지중, 충돌 오브젝트 위치 값, 감지 범위, 적 판별)
            Chr.isGaze = true;
        }
        else // 광선에 맞지 않았다면
        {
            Chr.isGaze = false;
        }
    }
}
