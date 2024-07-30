using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class swatAI : MonoBehaviour
{
    private Transform S_tr;
    private Transform P_tr;
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider CapCol;

    private WaitForSeconds Ws;
    private swatMoveAgent moveAgent;
    private swatFire swatfire;

    private readonly string P_tag = "Player";
    private readonly string s_tag = "SWAT";
    private readonly string u_tag = "Untagged";
    private readonly string hashIsMove = "IsMove";
    private readonly string hashMoveSpeed = "MoveSpeed";
    private readonly int hashDie = Animator.StringToHash("DieIdx");
    private readonly int hasDieTrigger = Animator.StringToHash("DieTrigger");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDieTrigger");

    private float ATK_dist = 5.0f;
    private float TRA_dist = 10.0f;
    public bool isDie = false;

    public enum S_State
    {
        PATOL = 0, TRACE, ATTACK, DIE, EXPDIE
    }
    public S_State state = S_State.PATOL;

    void Awake()
    {
        var PlayerObj = GameObject.FindWithTag(P_tag);
        if (PlayerObj != null)
            P_tr = PlayerObj.transform;
        S_tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CapCol = GetComponent<CapsuleCollider>();

        Ws = new WaitForSeconds(0.3f);

        moveAgent = GetComponent<swatMoveAgent>();
        swatfire =  GetComponent<swatFire>();
    }
    private void OnEnable()
    {
        Damage.OnPlayerDie += OnPlayerDie;
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 2.0f));
        StartCoroutine(StateCheck());
        StartCoroutine(PlayAction());
    }
    private void OnDisable()
    {
        Damage.OnPlayerDie -= OnPlayerDie;
    }
    IEnumerator StateCheck()
    {
        while (!isDie)
        {
            if(state == S_State.DIE || state == S_State.EXPDIE) yield break;
            float dist = (P_tr.position - S_tr.position).magnitude;
            if (dist <= ATK_dist)
                state = S_State.ATTACK;
            else if (dist <= TRA_dist)
                state = S_State.TRACE;
            else
                state = S_State.PATOL;

            yield return Ws;
        }
    }
    IEnumerator PlayAction()
    {
        while(!isDie)
        {
            yield return Ws;

            switch(state)
            {
                case S_State.PATOL:
                    animator.SetBool(hashIsMove, true);
                    moveAgent._Patoling = true;
                    swatfire.isFire = false;
                    break;

                case S_State.ATTACK:
                    animator.SetBool(hashIsMove, false);
                    swatfire.isFire = true;
                    moveAgent.Stop();
                    break;

                case S_State.TRACE:
                    animator.SetBool(hashIsMove, true);
                    moveAgent._traceTarget = P_tr.position;
                    swatfire.isFire = false;
                    break;

                case S_State.DIE:
                    S_Diestate();
                    break;

                case S_State.EXPDIE:
                    S_ExpDiestate();
                    break;
            }
        }

    }

    private void S_Diestate()
    {
        if (isDie) return;
        isDie = true;
        moveAgent.Stop();
        swatfire.isFire = false;
        animator.SetTrigger(hasDieTrigger);
        animator.SetInteger(hashDie, Random.Range(0, 3));
        rb.isKinematic = true;
        CapCol.enabled = false;
        gameObject.tag = u_tag;
        StartCoroutine(ObjPoolPush());
    }
    private void S_ExpDiestate()
    {
        if (isDie) return;
        isDie = true;
        moveAgent.Stop();
        swatfire.isFire = false;
        animator.SetTrigger(hasDieTrigger);
        animator.SetInteger(hashDie, 2);
        rb.isKinematic = true;
        CapCol.enabled = false;
        gameObject.tag = u_tag;
        StartCoroutine(ObjPoolPush());
    }
    IEnumerator ObjPoolPush()
    {
        yield return new WaitForSeconds(3.0f);
        isDie = false;
        rb.isKinematic = false;
        CapCol.enabled = true;
        gameObject.tag = s_tag;
        state = S_State.PATOL;
        gameObject.SetActive(false);
    }
    void OnPlayerDie()
    {
        StopAllCoroutines();//모든 코루틴 중지
        animator.SetTrigger(hashPlayerDie);
        GameManager.G_instance.isGameOver = true;
    }
    void Update()
    {
        animator.SetFloat(hashMoveSpeed, moveAgent.Speed);
    }
}
