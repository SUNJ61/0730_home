using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    private float Speed = 1500f;
    private Rigidbody rb;//����Ƽ���� ������ Rigibody�� �������� ����.
    private Transform tr;
    private TrailRenderer trail;

    public int damage = 20;
    void Awake()
    {               //Vector3.forward�� �۷ι� ��ǥ�� �̻��� �������� �Ѿ��� ���� �� �ִ�.
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        trail = GetComponent<TrailRenderer>();
        //Destroy(this.gameObject, 3.0f); // �ڱ��ڽ��� ������Ʈ�� 3���Ŀ� �޸𸮿��� �����Ѵ�, ������Ʈ Ǯ������ ��Ȱ��ȭ
    }
    private void DisableBullet()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        damage = (int)GameManager.Instance.gameData.damage;
        GameManager.OnItemChange += UpdateSetUp;
        rb.AddForce(transform.forward * Speed); //transform.forward(���� ��ǥ)�� ���ǵ� ��ŭ ������.
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
