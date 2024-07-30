using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCtrl : MonoBehaviour
{
    [Header("컴퍼넌트들")]
    public GameObject bulletPrefab; //총알 오브젝트 가져오기
    public Transform firePos; // 발사 위치 설정
    public Animation fireAni; // 총알 발사 할때 애니메이션
    public AudioSource fireSource; // 총알 발사 소리 출력 위치
    public AudioClip fireClip; //총알 발사 소리
    public ParticleSystem muzzleFlash; // 이펙트 설정
    private Image BulletImg;
    private Text BulletTxt;
    private AudioClip ReloadClip;

    [Header("각종 변수들")]
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
        //지금 이 스크립트가 있는 gameObject(오브젝트)에 존재하는 컴퍼넌트 HandCtrl를 handCtrl에 대입
        handCtrl = this.gameObject.GetComponent<HandCtrl>();
        BulletImg = GameObject.Find(UIObj).transform.GetChild(6).GetChild(2).GetComponent<Image>();
        BulletTxt = GameObject.Find(UIObj).transform.GetChild(6).GetChild(0).GetComponent<Text>();
        ReloadClip = Resources.Load<AudioClip>(ReloadSound);
        curbullet = maxbullet;
        fireTime = Time.time; // 현재시간을 대입
        muzzleFlash.Stop();
    }

  
    void Update()
    {
        #region 단발
        //0 마우스 왼쪽 버튼 눌렀을 때 / 1 마우스 오른쪽 버튼 / 2 마우스 휠
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Fire();
        //}
        #endregion
        #region 연사
            if (Input.GetMouseButton(0)) //누르는 동안 계속 호출된다. 딜레이가 없으면 오브젝트가 과생성.
            {
                if (Time.time - fireTime > 0.2f) // 현재시간에서 과거 시간을 뺀 시간이 0.1보다 클 경우
                {//즉, 위 조건은 0.1초의 딜레이를 주는 효과가 있다.
                    if (!handCtrl.isRun && !isReload)       
                            Fire();
                    fireTime = Time.time;
                }
            }
        #endregion
        //if(Input.GetMouseButtonUp(0)) //마우스 왼쪽 버튼을 띄웠을 때 발동
        //{
        //    muzzleFlash.Stop(); //Fire함수에 총구화염을 없애는 기능을 넣어서 필요없어짐.
        //}
    }
    void Fire()
    {
        ++bulletCount;
        --curbullet;
        //          무엇이        어디서            어떻게 회전하는지
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation); //오브젝트 생성 함수, 오브젝트 풀링으로 비활성화
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
        //       "매서드 명"         시간   => 원하는 시간간격 만큼 매서드를 호출 (0.03초마다 호출)
        UpdateBulletUI();
        if (bulletCount == 10)
        {
            StartCoroutine(Reload()); //게임중 개발자가 원하는 프레임을 만들려고 할 때 사용
            // 아래 IEnumerator Reload()를 호출한다.
        }
    }
    IEnumerator Reload()
    {
        isReload = true;
        fireAni.Play("pump3"); //리로드 애니메이션 재생
        fireSource.PlayOneShot(ReloadClip, 1.0f);
        yield return new WaitForSeconds(1.5f); //1.5초 후에 다음 반환 값으로 넘긴다. (1.5초후 다음 코드실행)
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

