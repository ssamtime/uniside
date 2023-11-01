using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;          // RigidBody 2D ������Ʈ
    float axisH = 0.0f;         // �¿� ����Ű �Է�
    public float speed = 3.0f;  // �̵��ӵ�

    public float jump = 9.0f;    // ������
    public LayerMask groundLayer;// ���� ������ ���̾�
    bool goJump = false;         // ���� Ű �Է� ����
    bool onGround = false;       // ����� ���� ����

    // �ִϸ�����
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    // ���� ����
    public static string gameState = "playing"; // �Լ� ����� ���� ����

    // ����
    public int score = 0;

    // ��ġ��ũ�� ���� ���� ����
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        // �h�� �� ��ũ��Ʈ�� ��ϵ� ������Ʈ�� RigidBody 2D ������Ʈ ���� ��������
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
        // ���� �÷��� ���� �ƴ� ��� ó������ �ʵ��� �Ѵ�
        if (gameState != "playing")
            return;

        // ���� ���� �Է�
        if(isMoving ==false)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }
        // ĳ���� ���� ����
        if(axisH>0.0f)
        {
            // ������ �̵�
            Debug.Log("������ �̵�");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // ���� �̵�
            Debug.Log("���� �̵�");
            transform.localScale = new Vector2(-1, 1);  // �¿� ����
        }

        // �ɸ��� ����
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }           
        
    }

    // (�ʱ⼳������) 0.02�ʸ��� ȣ��� (1�ʿ� 50��)
    // ��������ó���� ���⼭ �ؾߴ�
    void FixedUpdate()
    {
        // ���� �÷��� ���� �ƴ� ��� ó������ �ʵ��� �Ѵ�
        if (gameState != "playing")
            return;

        // ���� ���� ó��
        // ������, �������� ���� ���� ��� ���̾ �����ϸ� true ��ȯ
        onGround = Physics2D.Linecast(transform.position,
                                      transform.position - (transform.up * 0.1f),
                                      groundLayer);

        if(axisH!=0||onGround)
        {
            // ĳ���Ͱ� ���鿡 �ְų�, x�� �Է� ���� 0�� �ƴҰ��  ������Ʈ�� �ӵ� ����        
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y); // Vector2 (float x, float y)
        }
        if(onGround&&goJump)
        {
            // ĳ���Ͱ� ���鿡 ���� �� ���� Ű�� �Է����� ���
            Debug.Log("����!");
            Vector2 jumpPw = new Vector2(0, jump);       // ������ ���� ����
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); // �������� ���� ���Ѵ�
            goJump = false;                              // ���� �÷��� off
        }

        // �ִϸ��̼�
        if(onGround)
        {
            // ����� �´������ �� 
            if (axisH == 0)
                nowAnime = stopAnime; // ����
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

    // ���� Ʈ���� on
    public void Jump()
    {
        goJump = true;  //���� �÷��� on
        Debug.Log("���� Ű�� �Է�����!");
    }

    // ���� ���� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
            Goal();
        else if (collision.gameObject.tag == "Dead")
            GameOver(); 
        else if(collision.gameObject.tag=="ScoreItem")
        {
            // ���� ������ ���� ��������
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // ���� ���
            score = item.value;
            // ������ ����
            Destroy(collision.gameObject);
        }
    }

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop(); // ���� ����
    }

    // ���ӿ����϶� �ִϸ��̼� ���
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop();

        // ���� ���� ����
        // �÷��̾��� ���� �浹 ������ ��Ȱ��ȭ
        GetComponent<CapsuleCollider2D>().enabled = false;
        // �÷��̾ ��¦ ���� �̵���Ų��
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

    // ��ġ��ũ��(�����е�)���� ����� �Լ�
    public void SetAxis(float h,float v)
    {
        axisH = h;
        if (axisH == 0)
            isMoving = false;
        else
            isMoving = true;
    }
}


// Ű �Է� �Լ� ���
/*        
bool down = Input.GetKeyDown(KeyCode.Space);
bool press = Input.GetKey(KeyCode.Space);
bool up = Input.GetKeyUp(KeyCode.Space);

// ���콺 ��ư �� ��ġ �̺�Ʈ �˻�
// 0: ���� 1: ������ 2: �ٹ�ư
bool down = Input.GetMouseButtonDown(0);
bool press = Input.GetMouseButton(0);
bool up = Input.GetMouseButtonUp(0);

// Input Manager���� ������ ���ڿ��� ������� �ϴ� Ű �Է� �˻�
bool down = Input.GetButtonDown("Jump");
bool press = Input.GetButton("Jump");
bool up = Input.GetButtonUp("Jump");

// ������ �࿡ ���� Ű �Է�
float axisH = Input.GetAxis("Horizontal");  // -1 ~ 1
float axisV = Input.GetAxisRaw("Vertical"); // -1, 0, 1
*/