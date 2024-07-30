using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("컴포넌트")] //Attribute : 개발자가 쓰면 컴퓨터(유니티)가 읽어 해당 헤더명으로 나눠준다. 
    public NavMeshAgent agent; //추적할 대상을 찾는 네비
    public Transform Player; //추적할 대상과 추적자의 거리를 재기위해 사용
    public Transform thisZombie; 
    public Animator animator;
    public ZombieDamage zombieDamage;
    public Rigidbody rb;
    public CapsuleCollider capCol;

    private MOBFOV mobFOV;

    [Header("관련 변수")]
    public float attackDist = 3.0f; //공격 범위
    public float traceDist = 20f; //추적 범위

    public string playerDie = "PlayerDie";
    public string dieTrigger = "DieTrigger";
    void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>(); //자기자신 오브젝트에 안에 있는 nav컴퍼넌트를 적용시킨다.
        thisZombie = this.gameObject.GetComponent<Transform>(); //자기자신 오브젝트에 있는 transform컴퍼넌트를 적용시킨다.
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //하이라키안에 있는 게임오브젝트의 태그를 읽어서 가져온다. GetComponent<Transform>()는 transform으로 줄여 쓸 수 있다.
        animator = GetComponent<Animator>();//자기자신 오브젝트에서 컴포넌트를 가져올때 this.gameObject 생략가능
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        zombieDamage = GetComponent<ZombieDamage>();
        mobFOV = GetComponent<MOBFOV>();
    }
    private void OnEnable()
    {
        StartCoroutine(ZomAI());
    }
    IEnumerator ZomAI()
    {
        yield return new WaitForSeconds(1.0f);

        while (!(zombieDamage.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie))
        {
            ZomCtrl();
            yield return new WaitForSeconds(0.05f);
        }
        if(zombieDamage.isDie)
        {
            animator.SetTrigger(dieTrigger);
            agent.isStopped = true;
            capCol.enabled = false;
            rb.isKinematic = true;
            GameManager.Instance.KillScore();
            yield return new WaitForSeconds(3.0f);
            gameObject.SetActive(false);
        }
        if(Player.GetComponent<FPS_Damage>().isPlayerDie)
        {
            PlayerDeath();
        }
    }

    //void Update()
    //{
    //    if (zombieDamage.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie) return; //하위로직으로 내려가지않고 함수가 종료된다 즉, 업데이트가 종료됨
    //                                                                                     //거리를 잰다
    //    ZomCtrl();
    //}

    private void ZomCtrl()
    {
        float distance = Vector3.Distance(thisZombie.position, Player.position); //두 오브젝트의 3차원 좌표 거리를 저장한다.
        if (distance <= attackDist)
        {
            if (mobFOV.isViewPlayer())
            {
                Attack_ZOM();
            }
            else
                Trace_Zom();
        }
        else if (mobFOV.isTracePlayer())
        {
            Trace_Zom();
        }
        else
        {
            IDLE_ZOM();
        }
    }

    private void IDLE_ZOM()
    {
        agent.isStopped = true;
        animator.SetBool("isAttack", false);
        animator.SetBool("isTrace", false);
        //Debug.Log("추적 범위 벗어남");
    }

    private void Trace_Zom()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isTrace", true);
        agent.isStopped = false; //추적 시작
        agent.destination = Player.position; //추적 대상은 플레이어 좌표
                                             //Debug.Log("추적");
    }

    private void Attack_ZOM()
    {
        agent.isStopped = true; // 추적 중지
        animator.SetBool("isAttack", true);
        //Debug.Log("공격");
        Vector3 playerpos = Player.position - thisZombie.position; // 플레이어를 바라보는 방향, 거리를 얻는다.
        playerpos = playerpos.normalized; //방향만 저장
        Quaternion rot = Quaternion.LookRotation(playerpos);
        thisZombie.rotation = Quaternion.Slerp(thisZombie.rotation, rot, Time.deltaTime * 3.0f);
    }

    public void PlayerDeath()
    {
        GetComponent<Animator>().SetTrigger(playerDie);
    }
}
