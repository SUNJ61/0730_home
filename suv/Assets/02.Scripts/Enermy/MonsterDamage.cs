using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterDamage : MonoBehaviour
{
    public CapsuleCollider capsule;
    public Rigidbody rb;
    public Animator animator;
    public GameObject BLDeffect;
    public FireCtrl fireCtrl;
    public BoxCollider BoxCol;
    public MeshRenderer MeshRenderer;

    public string player = "Player";
    public string bullet = "BULLET";
    public string hitTrigger = "HitTrigger";
    public string dieTrigger = "DieTrigger";
    public bool isDie = false;

    public Image hpbar;
    public Text hptext;
    public int Maxhp = 100;
    public int Inithp = 0;

    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        fireCtrl = GameObject.FindWithTag(player).GetComponent<FireCtrl>();

        Inithp = Maxhp;
        hpbar.color = Color.green;
    }
    private void OnDisable()
    {
        capsule.enabled = true;
        rb.isKinematic = false;
        isDie = false;
        Inithp = Maxhp;
        hpbar.color = Color.green;
        UI_Ctrl();
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(player))
        {
            rb.mass = 800f;
            rb.freezeRotation = true;
            rb.isKinematic = false;
        }
        else if (col.gameObject.CompareTag(bullet))
        {
            hitInfo(col);

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
        hpbar.fillAmount = (float)Inithp / (float)Maxhp;
        hptext.text = $"HP : <color=#ff0000>{Inithp.ToString()}</color>";
        if (hpbar.fillAmount <= 0.3f)
            hpbar.color = Color.red;
        else if (hpbar.fillAmount <= 0.5f)
            hpbar.color = Color.yellow;
    }

    private void hitInfo(Collision col)
    {
        //Destroy(col.gameObject);
        col.gameObject.SetActive(false);
        animator.SetTrigger(hitTrigger);
        Vector3 hitpos = col.transform.position;
        Vector3 hitnormal = hitpos - fireCtrl.transform.position;
        hitnormal = hitnormal.normalized;
        Quaternion hitrot = Quaternion.LookRotation(hitnormal);
        var blood = Instantiate(BLDeffect, hitpos, hitrot);
        Destroy(blood, Random.Range(0.8f, 1.2f));
    }

    void Die()
    {
        //animator.SetTrigger(dieTrigger);
        //capsule.enabled = false;
        //rb.isKinematic = true;
        isDie = true;
        //GameManager.Instance.KillScore(1);
        //gameObject.SetActive(false);
    }
    void ExpHp()
    {
        Inithp = 0;
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
