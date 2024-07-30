using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    private float Speed = 1500f;
    private Rigidbody rb;//유니티에서 설정한 Rigibody를 가져오기 위함.
    private Transform tr;
    private TrailRenderer trail;

    public int damage = 20;
    void Awake()
    {               //Vector3.forward는 글로벌 좌표라 이상한 방향으로 총알이 나갈 수 있다.
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        trail = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 3.0f); // 자기자신의 오브젝트를 3초후에 메모리에서 삭제한다, 오브젝트 풀링으로 비활성화
    }
    private void DisableBullet()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        damage = (int)GameManager.Instance.gameData.damage;
        GameManager.OnItemChange += UpdateSetUp;
        rb.AddForce(transform.forward * Speed); //transform.forward(로컬 좌표)로 스피드 만큼 나간다.
        Invoke("DisableBullet", 2.0f);
    }
    void UpdateSetUp()
    {
        damage = (int)GameManager.Instance.gameData.damage;
    }
    private void OnDisable()
    {
        trail.Clear();
        //tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
