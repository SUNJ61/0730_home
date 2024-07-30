using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Utility;

public class SkeletonCtrl : MonoBehaviour
{
    [Header("컴포넌트")]
    public Rigidbody rb;
    public CapsuleCollider capCol;
    public Transform Skel;
    public Transform Player;
    public NavMeshAgent nav;
    public Animator animator;
    public SkeletonDamage Skel_D;

    private MOBFOV mobFOV;

    [Header("사용 변수")]
    public string player = "Player";
    public string At = "isAttack";
    public string Tr = "isTrace";
    public string playerDie = "PlayerDie";
    public string die_T = "dieTrigger";
    public float AttackDis = 4.0f;
    public float TraceDis = 20.0f;

    void Awake()
    {
        Skel = gameObject.GetComponent<Transform>();
        Player = GameObject.FindWithTag(player).GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Skel_D = GetComponent<SkeletonDamage>();
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();

        mobFOV = GetComponent<MOBFOV>();
    }
    private void OnEnable()
    {
        StartCoroutine(SkelAI());
    }
    IEnumerator SkelAI()
    {
        yield return new WaitForSeconds(1.0f);

        while (!(Skel_D.isDie || Player.GetComponent<FPS_Damage>().isPlayerDie))
        {
            SkelCtrl();
            yield return new WaitForSeconds(0.00005f);
        }
        if(Skel_D.isDie)
        {
            animator.SetTrigger(die_T);
            nav.isStopped = true;
            rb.isKinematic = true;
            capCol.enabled = false;
            GameManager.Instance.KillScore();
            yield return new WaitForSeconds(3.0f);
            gameObject.SetActive(false);
        }
        if (Player.GetComponent<FPS_Damage>().isPlayerDie)
        {
            PlayerDeath();
        }
    }

    private void SkelCtrl()
    {
        float Dist = Vector3.Distance(Player.position, Skel.position);

        if (Dist <= AttackDis)
        {
            if (mobFOV.isViewPlayer())
                Attack_Skel();
            else
                Trace_Skel();

        }
        else if (mobFOV.isTracePlayer())
        {
            Trace_Skel();
        }
        else if (Dist > TraceDis)
        {
            IDLE_Skel();
        }
    }

    private void IDLE_Skel()
    {
        nav.isStopped = true;
        animator.SetBool(At, false);
        animator.SetBool(Tr, false);
    }

    private void Trace_Skel()
    {
        nav.isStopped = false;
        animator.SetBool(At, false);
        animator.SetBool(Tr, true);
        nav.destination = Player.position;
    }

    private void Attack_Skel()
    {
        nav.isStopped = true;
        animator.SetBool(At, true);
        Vector3 playerpos = (Player.position - Skel.position).normalized;
        //Quaternion rot = Quaternion.FromToRotation(Skel.position, playerpos); // 파티클을 조정할때 주로 사용.
        Quaternion rot = Quaternion.LookRotation(playerpos);
        Skel.rotation = Quaternion.Slerp(Skel.rotation, rot, Time.deltaTime * 3.0f);
    }

    public void PlayerDeath()
    {
        GetComponent<Animator>().SetTrigger(playerDie);
    }
}
