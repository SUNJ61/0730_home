using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieDamage : MonoBehaviour
{
    [Header("컴퍼넌트")]
    public Rigidbody rb;
    public CapsuleCollider capCol;
    public Animator animator;
    public GameObject BloodEffect;
    public FireCtrl fireCtrl;
    public BoxCollider BoxCol;
    public MeshRenderer MeshRenderer;

    [Header("사용 변수")]
    public string playerTag = "Player"; //이것은 "Player" 문자열을 동적할당 한것이다.
    public string bulletTag = "BULLET";
    public string hitTrigger = "HitTrigger";
    public string dieTrigger = "DieTrigger";
    public int hitCount = 0;
    public bool isDie = false; // 죽음 감지했을 때 true로 업데이트

    [Header("UI")]
    public Image hpBar;
    public Text hpTxt;
    public int maxHp = 100;
    public int HpInit = 0; //초기값
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
        capCol = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        fireCtrl = GameObject.FindWithTag(playerTag).GetComponent<FireCtrl>();

        HpInit = maxHp;
        hpBar.color = Color.green;
    }
    private void OnDisable()
    {
        capCol.enabled = true;
        rb.isKinematic = false; 
        isDie = false;
        HpInit = maxHp;
        hpBar.color = Color.green;
        UI_Ctrl();
    }
    private void OnCollisionEnter(Collision col) //충돌을 감지할 때 isTrigger체크 안했을때 사용
    {
        if(col.gameObject.CompareTag(playerTag)) //col.gameObject.CompareTag == "Player"은 동적할당과 비교를 동시에 실행, 런타임에서 할일이 많음 메모리, 속도등 다양한 방면에서 불리함.
        {//동적 할당과 비교를 따로하는 이 방법을 더 주로 쓴다.
            rb.mass = 800f; //플레이와 충돌하면 무게가 5000으로 늘어난다. -> 플레이어와 충돌하더라도 안밀리도록 설정
            rb.freezeRotation = true; //부딫히면 회전 제한하기.
            rb.isKinematic = false; //부딫히면 물리력이 있게 설정 (안밀리도록 설정)
        }
        else if(col.gameObject.CompareTag(bulletTag))
        {
            HitInfo(col);
            HpInit -= col.gameObject.GetComponent<BulletCtrl>().damage; //같은 오브젝트 안에있는 총알 스크립트에 데미지 변수를 가져온다.
            UI_Ctrl();

            if (++hitCount == 5)
            {
                if (!isDie)
                    Die();
            }
        }
    }

    private void UI_Ctrl()
    {
        hpBar.fillAmount = (float)HpInit / (float)maxHp; //fillAmount는 플롯값만 받는다.
        hpTxt.text = $"Hp : <color=#ff0000>{HpInit.ToString()}</color>";
        if (hpBar.fillAmount <= 0.3f)
            hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow;
    }

    private void HitInfo(Collision col)
    {
        //Print("아야");
        //Destroy(col.gameObject); //충돌한 오브젝트 삭제
        col.gameObject.SetActive(false);
        animator.SetTrigger(hitTrigger);

        Vector3 hitpos = col.transform.position; // 맞은 위치를 3차원 공간에 저장
        Vector3 hitnormal = hitpos - fireCtrl.transform.position;
        hitnormal = hitnormal.normalized;
        Quaternion hitrot = Quaternion.LookRotation(hitnormal);
        //Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitpos.normalized); // 일정 범위 만큼 회전하게 설정하는 명령어, 앞쪽 방향에서 맞은 방향으로 회전
        var blood = Instantiate(BloodEffect, hitpos, hitrot);
        //오브젝트 생성 (충돌을 감지한 오브젝트에서 회전 없이.), var = GameObject
        Destroy(blood, Random.Range(0.8f, 1.2f)); // 0.8~1.2초 랜덤한 시간 이후 생성한 blood 자료형 삭제
    }

    private void OnCollisionExit(Collision col)
    {
        rb.mass = 75f;
    }

    void Die()
    {
        //animator.SetTrigger(dieTrigger);
        //capCol.enabled = false; //콜라이더 비활성화
        //rb.isKinematic = true; //오브젝트의 물리력을 제거함.
        isDie = true;
        //GameManager.Instance.KillScore(1);
        //gameObject.SetActive(false);
    }
    void ExpHp()
    {
        HpInit = 0;
        UI_Ctrl();
    }
    public void BoxColEnable()
    {
        BoxCol.enabled = true;
        //MeshRenderer.enabled = true;
    }
    public void BoxColDisable()
    {
        BoxCol.enabled = false;
        //MeshRenderer.enabled = false;
    }
}
