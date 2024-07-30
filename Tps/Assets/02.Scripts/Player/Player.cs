using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] // public���� ����� ��� �ʵ带 �ν����� â�� �����ش�.
public class PlayerAniMation //�ִϸ��̼� �̸��� �����ϰ� �� �� �ְ�, ���羲�� �ִϸ��̼��� �������� �� �� �ִ�.
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
        GameManager.OnItemChange += UpdateSetUp; //�̺�Ʈ�� ����Ѵ�. �ش� �̺�Ʈ�� �κ��丮�� �������� �߰��ǰų� ������ �ߵ�.
    }
    void UpdateSetUp()
    {
        moveSpeed = GameManager.G_instance.gameData.speed;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();//�̿� ���� ������� ���۳�Ʈ ĳ��ó����� �Ѵ�.
        capCol = rb.GetComponent<CapsuleCollider>();
        tr = rb.GetComponent<Transform>();
        _animation = GetComponent<Animation>();
        _animation.Play(playerAnimation.idle.name); // �ִϸ��̼� idle�ൿ�� string ���� ��������

        moveSpeed = GameManager.G_instance.gameData.speed;
    }
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        //Space.Self�� ���� ��ǥ�� �������� �ǹ��Ѵ�.(�ٶ󺸴� ������) world�� ���� ���� ��ǥ -> �̵��� ������ ���� �������� �̵�
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
            _animation.CrossFade(playerAnimation.runRight.name, 0.3f); //���� �ִϿ� ���� ������ �ִϸ� 0.3�ʰ� ȥ���Ͽ� �ε巴�� ���
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
