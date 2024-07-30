using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // public으로 선언된 멤버 필드를 인스펙터 창에 보여준다.
public class PlayerAniMation //애니메이션 이름을 간략하게 할 수 있고, 현재쓰는 애니메이션이 무엇인지 알 수 있다.
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
    public AnimationClip Sprint;
}
public class Player : MonoBehaviour
{
    public PlayerAniMation playerAnimation;

    private Rigidbody rb;
    private CapsuleCollider capCol;
    private Transform tr;
    private Animation _animation;

    [SerializeField]private float moveSpeed;
    private float rotSpeed = 90f;
    private float h, v, r;

    public bool isRun = false;
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp; //이벤트를 등록한다. 해당 이벤트는 인벤토리의 아이템이 추가되거나 빠지면 발동.
    }
    void UpdateSetUp()
    {
        moveSpeed = GameManager.G_instance.gameData.speed;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();//이와 같은 선언들을 컴퍼넌트 캐시처리라고 한다.
        capCol = rb.GetComponent<CapsuleCollider>();
        tr = rb.GetComponent<Transform>();
        _animation = GetComponent<Animation>();
        _animation.Play(playerAnimation.idle.name); // 애니메이션 idle행동의 string 값을 전달해줌

        moveSpeed = GameManager.G_instance.gameData.speed;
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        //Space.Self는 로컬 좌표로 움직임을 의미한다.(바라보는 방향대로) world를 쓰면 절대 좌표 -> 이동이 고정된 축을 기준으로 이동
        {
            MoveAni();
            RunAni();
        }
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * r);
    }

    private void RunAni()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = GameManager.G_instance.gameData.speed + 6f;
            _animation.CrossFade(playerAnimation.Sprint.name, 0.3f);
            isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = GameManager.G_instance.gameData.speed;
            _animation.CrossFade(playerAnimation.runForward.name, 0.3f);
            isRun = false;
        }
    }

    private void MoveAni()
    {
        if (h > 0.1f)
            _animation.CrossFade(playerAnimation.runRight.name, 0.3f); //이전 애니와 다음 동작할 애니를 0.3초간 혼합하여 부드럽게 출력
        else if (h < -0.1f)
            _animation.CrossFade(playerAnimation.runLeft.name, 0.3f);
        else if (v > 0.1f)
            _animation.CrossFade(playerAnimation.runForward.name, 0.3f);
        else if (v < -0.1f)
            _animation.CrossFade(playerAnimation.runBackward.name, 0.3f);
        else
            _animation.CrossFade(playerAnimation.idle.name, 0.3f);
    }
}
