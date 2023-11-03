using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �̵��ӵ�
    public float speed = 3.0f;
    
    // �ִϸ��̼�
    public string upAnime = "PlayerUp";
    public string downAnime = "PlayerDown";
    public string leftAnime = "PlayerLeft";
    public string rightAnime = "PlayerRight";
    public string deadAnime = "PlayerDead";

    // ���� �ִϸ��̼�
    string nowAnimation = "";
    // ���� �ִϸ��̼�
    string oldAnimation = "";

    float axisH = 0.0f;                     // ���� �Է� (-1.0 ~ 1.0)
    float axisV = 0.0f;                     // ���� �Է� (-1.0 ~ 1.0)
    public float angleZ = -90.0f; // ȸ��

    Rigidbody2D rbody;              // RigidBody 2D ������Ʈ
    bool isMoving = false;          // �̵� ��

    // Start is called before the first frame update
    void Start()
    {
        // ���� �� ��ũ��Ʈ�� ��ϵ� ������Ʈ�� RigidBody 2D ������Ʈ ���� ��������
        rbody = this.GetComponent<Rigidbody2D>();

        // �ִϸ����� ���� ��������
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        // ���� ���� (�÷��� ��)
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �÷��� ���� �ƴ� ��쿡�� ó������ �ʵ��� �Ѵ�
        if (gameState != "playing")
            return;

        // ���� ���� �Է�
        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }

        // ĳ���� ���� ����
        if (axisH > 0.0f)
        {
            // ������ �̵�
            Debug.Log("������ �̵�");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // ���� �̵�
            Debug.Log("���� �̵�");
            transform.localScale = new Vector2(-1, 1); // �¿� ����
        }

        // ĳ���� ����
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // (����Ƽ �ʱ� ���� ����) 0.02�ʸ��� ȣ��Ǹ�, 1�ʿ� �� 50�� ȣ��Ǵ� �Լ�
    void FixedUpdate()
    {
        // ���� �÷��� ���� �ƴ� ��쿡�� ó������ �ʵ��� �Ѵ�
        if (gameState != "playing")
            return;

        // ���� ���� ó��
        onGround = Physics2D.Linecast(transform.position,
                                                                    transform.position - (transform.up * 0.1f),
                                                                    groundLayer);

        if (onGround || axisH != 0)
        {
            // ĳ���Ͱ� ���鿡 �ְų� X�� �Է� ���� 0�� �ƴ� ��� velocity ����
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
        }
        if (onGround && goJump)
        {
            // ĳ���Ͱ� ���鿡 ���� �� ���� Ű�� �Է����� ���
            Debug.Log("����!");
            Vector2 jumpPw = new Vector2(0, jump);                     // ������ ���� ����
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    // �������� ���� ���Ѵ�
            goJump = false;                                                                    // ���� �÷��� OFF
        }

        // �ִϸ��̼�
        if (onGround)
        {
            // ����� �´������ ��
            if (axisH == 0)
                nowAnime = stopAnime;   // ����
            else
                nowAnime = moveAnime; // �̵�
        }
        else
        {
            // ���߿� ���� ��
            nowAnime = jumpAnime; // ����
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // �ִϸ��̼� ���
        }
    }

    // ���� Ʈ���� ON
    public void Jump()
    {
        goJump = true;  // ���� �÷��� ON
        Debug.Log("���� Ű�� �Է�����!");
    }

    // ���� ���� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
            Goal(); // ����
        else if (collision.gameObject.tag == "Dead")
            GameOver(); // ���ӿ���
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // ���� ������ ���� ��������
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // ���� ���
            score = item.value;
            // ������ ����
            Destroy(collision.gameObject);
        }
    }

    // �� ������ �������� ���� �ִϸ��̼� ���
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop(); // ���� ����
    }

    // ���� ������ ���� �ִϸ��̼� ���
    public void GameOver()
    {
        animator.Play(deadAnime);

        // ���� ���� ó��
        gameState = "gameover";
        GameStop();

        // ���� ���� ����
        // �÷��̾��� �浹 ������ ��Ȱ��ȭ
        GetComponent<CapsuleCollider2D>().enabled = false;
        // �÷��̾ ��¦ ���� ����ش�
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    // ���� ����
    void GameStop()
    {
        // rigidBody2D ���� ��������
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        // �ӵ��� 0���� �Ͽ� ������ ���߰� �Ѵ�
        rbody.velocity = new Vector2(0, 0);
    }

    // ��ġ��ũ��(���� �е�)���� ����� �Լ�
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
            isMoving = false;
        else
            isMoving = true;
    }
}

// Ű �Է� ���� �Լ� ���
/*
    // Ű������ Ư�� Ű �Է¿� ���� �˻�
    bool down = Input.GetKeyDown(KeyCode.Space);
    bool press = Input.GetKey(KeyCode.Space);
    bool up = Input.GetKeyUp(KeyCode.Space);

    // ���콺 ��ư �Է� �� ��ġ �̺�Ʈ�� ���� �˻�
    // 0 : ���콺 ���� ��ư
    // 1 : ���콺 ������ ��ư
    // 2 : ���콺 �� ��ư
    bool down = Input.GetMouseButtonDown(0);
    bool press = Input.GetMouseButton(0);
    bool up = Input.GetMouseButtonUp(0);

    // Input Manager���� ������ ���ڿ��� ������� �ϴ� Ű �Է� �˻�
    bool down = Input.GetButtonDown("Jump");
    bool press = Input.GetButton("Jump");
    bool up = Input.GetButtonUp("Jump");

    // ������ �࿡ ���� Ű �Է� �˻�
    float axisH = Input.GetAxis("Horizontal");
    float axisV = Input.GetAxisRaw("Vertical");
*/