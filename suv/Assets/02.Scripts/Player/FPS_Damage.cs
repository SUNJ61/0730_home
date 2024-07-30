using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPS_Damage : MonoBehaviour
{
    public Image HpBar;
    public Text HpText;
    public GameObject ScreenImg;

    private int initHP = 0;
    private int Maxhp = 100;

    private int SK_dam = 10;
    private int ZOM_dam = 1;
    private int MON_dam = 5;

    private string SK_attack_tag = "ATTACK_SK";
    private string ZOM_attack_tag = "ATTACK_ZOM";
    private string MON_attack_tag = "ATTACK_MON";

    public bool isPlayerDie = false;
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }
    void UpdateSetUp()
    {
        Maxhp = (int)GameManager.Instance.gameData.hp;
        initHP = (int)GameManager.Instance.gameData.hp - Maxhp;
    }
    void Start()
    {
        ScreenImg = GameObject.Find("Canvas-UI").transform.GetChild(5).gameObject;
        initHP = Maxhp;
        HpBar.color = Color.green;
        HPinfo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(SK_attack_tag))
        {
            SK_Dam_hp();
        }
        if(other.gameObject.CompareTag(ZOM_attack_tag))
        {
            ZOM_Dam_hp();
        }
        if (other.gameObject.CompareTag(MON_attack_tag))
        {
            MON_Dam_hp();
        }
        if (initHP <= 0)
        {
            PlayerDie();
        }
    }

    private void MON_Dam_hp()
    {
        initHP -= MON_dam;
        HpBarctrl();
    }

    private void ZOM_Dam_hp()
    {
        initHP -= ZOM_dam;
        HpBarctrl();
    }

    private void SK_Dam_hp()
    {
        initHP -= SK_dam;
        HpBarctrl();
    }

    private void HPinfo()
    {
        HpText.text = $"HP : <color=#FF0000>{initHP.ToString()}</color>";
    }

    void HpBarctrl()
    {
        initHP = Mathf.Clamp(initHP, 0, Maxhp);
        HpBar.fillAmount = (float)initHP / (float)Maxhp;
        if (HpBar.fillAmount <= 0.3)
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5)
            HpBar.color = Color.yellow;
        HpText.text = $"HP : <color=#FF0000>{initHP.ToString()}</color>";
    }

    void PlayerDie()
    {
        ScreenImg.SetActive(true);

        isPlayerDie = true;

        GameObject[] mobs = GameObject.FindGameObjectsWithTag("MOB");//��Ÿ�ӿ��� MOB��� �±׸� ���� ������Ʈ���� mobs�迭�� ����.
        for(int i = 0; i < mobs.Length; i++)
        {
            mobs[i].gameObject.SendMessage("PlayerDeath",SendMessageOptions.DontRequireReceiver);
            //�ٸ� ���ӿ�����Ʈ�� �ִ� �żҵ带 ȣ���ϴ� ����� ���� ��ɾ�
            //SendMessageOptions.DontRequireReceiver�� string�� ��Ÿ���ְų� �����Լ����� ���� ��� x
        }
        Invoke("MoveNextScene", 3.0f);
    }

    void MoveNextScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
