using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [Header("���۳�Ʈ��")]
    public GameObject bulletPrefab; //�Ѿ� ������Ʈ ��������
    public Transform firePos; // �߻� ��ġ ����
    public Animation fireAni; // �Ѿ� �߻� �Ҷ� �ִϸ��̼�
    public AudioSource fireSource; // �Ѿ� �߻� �Ҹ� ��� ��ġ
    public AudioClip fireClip; //�Ѿ� �߻� �Ҹ�
    public ParticleSystem muzzleFlash; // ����Ʈ ����
    private Image BulletImg;
    private Text BulletTxt;
    private AudioClip ReloadClip;

    [Header("���� ������")]
    public float fireTime;
    public HandCtrl handCtrl;
    public int bulletCount = 0;
    bool isReload = false;
    private readonly string UIObj = "Canvas-UI";
    private readonly string ReloadSound = "p_shotgun_reload";
    private int maxbullet = 10;
    private int curbullet;

    void Start()
    {
        //���� �� ��ũ��Ʈ�� �ִ� gameObject(������Ʈ)�� �����ϴ� ���۳�Ʈ HandCtrl�� handCtrl�� ����
        handCtrl = this.gameObject.GetComponent<HandCtrl>();
        BulletImg = GameObject.Find(UIObj).transform.GetChild(6).GetChild(2).GetComponent<Image>();
        BulletTxt = GameObject.Find(UIObj).transform.GetChild(6).GetChild(0).GetComponent<Text>();
        ReloadClip = Resources.Load<AudioClip>(ReloadSound);
        curbullet = maxbullet;
        fireTime = Time.time; // ����ð��� ����
        muzzleFlash.Stop();
    }

  
    void Update()
    {
        #region �ܹ�
        //0 ���콺 ���� ��ư ������ �� / 1 ���콺 ������ ��ư / 2 ���콺 ��
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Fire();
        //}
        #endregion
        #region ����
            if (Input.GetMouseButton(0)) //������ ���� ��� ȣ��ȴ�. �����̰� ������ ������Ʈ�� ������.
            {
                if (Time.time - fireTime > 0.2f) // ����ð����� ���� �ð��� �� �ð��� 0.1���� Ŭ ���
                {//��, �� ������ 0.1���� �����̸� �ִ� ȿ���� �ִ�.
                    if (!handCtrl.isRun && !isReload)       
                            Fire();
                    fireTime = Time.time;
                }
            }
        #endregion
        //if(Input.GetMouseButtonUp(0)) //���콺 ���� ��ư�� ����� �� �ߵ�
        //{
        //    muzzleFlash.Stop(); //Fire�Լ��� �ѱ�ȭ���� ���ִ� ����� �־ �ʿ������.
        //}
    }
    void Fire()
    {
        ++bulletCount;
        --curbullet;
        //          ������        ���            ��� ȸ���ϴ���
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation); //������Ʈ ���� �Լ�, ������Ʈ Ǯ������ ��Ȱ��ȭ
        var _bullet = PoolingManager.P_instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        fireSource.PlayOneShot(fireClip, 1.0f);
        fireAni.Play("fire");
        muzzleFlash.Play();
        Invoke("MuzzleFlashDisable", 0.03f);
        //       "�ż��� ��"         �ð�   => ���ϴ� �ð����� ��ŭ �ż��带 ȣ�� (0.03�ʸ��� ȣ��)
        UpdateBulletUI();
        if (bulletCount == 10)
        {
            StartCoroutine(Reload()); //������ �����ڰ� ���ϴ� �������� ������� �� �� ���
            // �Ʒ� IEnumerator Reload()�� ȣ���Ѵ�.
        }
    }
    IEnumerator Reload()
    {
        isReload = true;
        fireAni.Play("pump3"); //���ε� �ִϸ��̼� ���
        fireSource.PlayOneShot(ReloadClip, 1.0f);
        yield return new WaitForSeconds(1.5f); //1.5�� �Ŀ� ���� ��ȯ ������ �ѱ��. (1.5���� ���� �ڵ����)
        bulletCount = 0;
        curbullet = maxbullet;
        UpdateBulletUI();
        isReload = false;
    }

    private void UpdateBulletUI()
    {
        BulletImg.fillAmount = (float)curbullet / (float)maxbullet;
        BulletTxt.text = string.Format($"<color=#ff0000>{curbullet}</color>/{maxbullet}");
    }

    void MuzzleFlashDisable()
    {
        muzzleFlash.Stop();
    }
}

