using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonDamage : MonoBehaviour
{
    [Header("컴포넌트")]
    public Rigidbody rb;
    public CapsuleCollider capCol;
    public Animator animator;
    public GameObject BLDEF;
    public FireCtrl fireCtrl; //플레이어 발사위치를 가져온다.
    public BoxCollider boxCol;
    public MeshRenderer meshRender;
    public AudioSource audioSource;
    public AudioClip swordclip;

    [Header("사용 변수")]
    public string Player = "Player";
    public string Bt = "BULLET";
    public string hit_T = "hitTrigger";
    public string die_T = "dieTrigger";
    public bool isDie = false; 
    public int Mhp = 100;
    public int Inithp = 0;

    [Header("UI")]
    public Image hp_image;
    public Text hp_text;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        fireCtrl = GameObject.FindWithTag(Player).GetComponent<FireCtrl>(); // 하이라키 안에 플레이어 태그를 찾아 firectrl클래스를 찾는다.
        audioSource = GetComponent<AudioSource>();

        hp_image.color = Color.green;
        Inithp = Mhp;
    }
    private void OnDisable()
    {
        capCol.enabled = true;
        rb.isKinematic = false;
        isDie = false;
        Inithp = Mhp;
        UI_Ctrl();
        hp_image.color = Color.green;
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(Player))
        {
            rb.mass = 800f;
            //rb.freezeRotation = true;
            rb.isKinematic = false;
        }
        else if(col.gameObject.CompareTag(Bt))
        {
            animator.SetTrigger(hit_T);
            HitInfo(col);
            Inithp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            UI_Ctrl();

            if (Inithp <= 0)
            {
                if (!isDie)
                    Die();
            }
        }
    }

    private void UI_Ctrl()
    {
        hp_image.fillAmount = (float)Inithp / (float)Mhp;
        hp_text.text = $"HP : <color=#ff0000>{Inithp.ToString()}</color>";
        if (Inithp < 30)
            hp_image.color = Color.red;
        else if (Inithp < 50)
            hp_image.color = Color.yellow;
    }

    private void Die()
    {
        //animator.SetTrigger(die_T);
        //rb.isKinematic = true;
        //capCol.enabled = false;
        isDie = true;
        //GameManager.Instance.KillScore(1);
        //gameObject.SetActive(false);
    }
    void ExpHp()
    {
        Inithp = 0;
        UI_Ctrl();
    }
    private void HitInfo(Collision col)
    {
        //Destroy(col.gameObject);
        col.gameObject.SetActive(false);
        Vector3 hitpos = col.transform.position;
        Vector3 hitnormal = hitpos - fireCtrl.transform.position; // 맞은위치 - 발사 워치 (맞은 거리 및 방향의 복합정보.)
        hitnormal = hitnormal.normalized; //방향의 정보만 남긴다. (백터만 남음)
        Quaternion rot = Quaternion.LookRotation(hitnormal); // 백터를 입력 받아 그방향으로 회전시킨다.

        GameObject Blood = Instantiate(BLDEF, hitpos,rot);
        Destroy(Blood, Random.Range(0.8f, 1.2f));
    }

    public void BoxColEnable()
    {
        boxCol.enabled= true;
        //meshRender.enabled = true;
    }
    public void BoxColDisable()
    {
        boxCol.enabled = false;
        //meshRender.enabled = false;
    }

    public void Sword_Sfx()
    {
        audioSource.clip = swordclip;
        audioSource.PlayDelayed(0.15f);
    }
}
