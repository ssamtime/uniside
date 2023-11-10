using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 3.0f;  // �̵��ӵ�

    public float jump = 9.0f;    // ������
    public LayerMask groundLayer;// ���� ������ ���̾�

    // �ִϸ��̼�
    public string upAnime = "PlayerUp";
    public string downAnime = "PlayerDown";
    public string leftAnime = "PlayerLeft";
    public string rightAnime = "PlayerRight";
    public string deadAnime = "PlayerDead";

    string nowAnimation = "";       // ���� �ִϸ��̼�
    string oldAnimation = "";       // ���� �ִϸ��̼�

    float axisH = 0.0f;         // ���� �Է� (-1.0 ~ 1.0)
    float axisV = 0.0f;         // ���� �Է� (-1.0 ~ 1.0)
    public float angleZ = -90.0f; // ȸ��

    Rigidbody2D rbody;          // RigidBody 2D ������Ʈ
    bool isMoving = false;      // �̵� �� 

    public static int hp = 3;       // �÷��̾��� HP
    public static string gameState; // ���� ����
    bool inDamage = false;          // �ǰݻ���


    // Start is called before the first frame update
    void Start()
    {
        // �h�� �� ��ũ��Ʈ�� ��ϵ� ������Ʈ�� RigidBody 2D ������Ʈ ���� ��������
        rbody = this.GetComponent<Rigidbody2D>();

        // (�⺻)�ִϸ��̼� ����
        oldAnimation = downAnime;

        // ���� ���� ����
        gameState = "playing";

        // HP �ҷ�����
        hp = PlayerPrefs.GetInt("PlayerHP");
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ���� �ƴϰų� ���ݹް� ���� ��쿡�� �ƹ��͵� ���� ����
        if (gameState != "playing" || inDamage)
        {
            return;
        }

        if(isMoving==false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); //�¿�
            axisV = Input.GetAxisRaw("Vertical");   //���� (-1~1)
        }

        // Ű�Է��� ���Ͽ� �̵� ���� ���ϱ�
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        // �̵� ������ �������� ����� �ִϸ��̼��� �����Ѵ�
        if(angleZ >= -45&&angleZ<45)
        {
            // ������
            nowAnimation = rightAnime;
        }
        else if(angleZ>=45&& angleZ<=135)
        {
            // ����
            nowAnimation = upAnime;
        }
        else if(angleZ>=-135&& angleZ<-45)   
        {
            // �Ʒ���
            nowAnimation = downAnime;
        }
        else 
        {
            // ����
            nowAnimation = leftAnime;
        }

        // �ִϸ��̼� ����
        if(nowAnimation !=oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation);
        }
    }

    // (�ʱ⼳������) 0.02�ʸ��� ȣ��� (1�ʿ� 50��)
    // ��������ó���� ���⼭ �ؾߴ�
    void FixedUpdate()
    {
        // �������� �ƴϸ� �ƹ��͵� ���� ����
        if(gameState != "playing")
        {
            return;
        }

        // ���ݹ޴� ���߿� ĳ���͸� �����Ų��
        if(inDamage)
        {
            // Time.time : ���� ���ۺ��� ��������� ����ð� (�ʴ���)
            // Sin �Լ��� ���������� �����ϴ� ���� �����ϸ� 0~1~0~-1~0... ������ ����
            float value = Mathf.Sin(Time.time * 50);
            Debug.Log(value);
            if(value>0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return; // �ǰ��� �ƹ�������ϰ���
        }

        // �̵� �ӵ��� ���Ͽ� ĳ���͸� �������ش�
        rbody.velocity = new Vector2(axisH, axisV) * speed;
    }

    // ��ġ��ũ��(�����е�)���� ����� �Լ�
    public void SetAxis(float h,float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV ==0)
            isMoving = false;
        else
            isMoving = true;
    }

    // p1���� p2������ ������ ����Ѵ�
    float GetAngle(Vector2 p1,Vector2 p2)
    {
        float angle;

        // �� ���⿡ ������� ĳ���Ͱ� �����̰� ���� ���
        if(axisH!=0 || axisV!=0)
        {
            // p1�� p2�� ���� ���ϱ� (������ 0���� �ϱ� ����)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;

            // ��ũź��Ʈ �Լ��� ����(����) ���ϱ�
            float rad = Mathf.Atan2(dy, dx);

            // ���ȿ��� ������ ��ȯ
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // ĳ���Ͱ� ���� ���̸� ���� ����
            angle = angleZ;
        }
        return angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy�� ���������� �浹 �߻�
        if(collision.gameObject.tag == "Enemy")
        {
            // ������ ���
            GetDamage(collision.gameObject);
        }
    }

    void GetDamage(GameObject enemy)
    {
        if(gameState == "playing")
        {
            hp--;   //HP����
            PlayerPrefs.SetInt("PlayerHP", hp); //���� HP ����

            if(hp>0)
            {
                // �̵� ����
                rbody.velocity = new Vector2(0, 0);
                // ��Ʈ�� (���� ������ ������ �ݴ��)
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x * 4, toPos.y * 4), ForceMode2D.Impulse);
                // ���� ���ݹް� ����
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                // ü���� ������ ���ӿ���
                GameOver();
            }
        }
    }

    // ������ ó�� ����
    void DamageEnd()
    {
        inDamage = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // ���ӿ��� ó��
    void GameOver()
    {
        Debug.Log("���ӿ���!");
        gameState = "gameover";

        // ���ӿ��� ����
        // �浹 ���� ��Ȱ��ȭ
        GetComponent<CircleCollider2D>().enabled = false;
        // �̵� ����
        rbody.velocity = new Vector2(0, 0);
        // �߷��� ���� �÷��̾ ���� ��¦ ����
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        // �ִϸ��̼� ����
        GetComponent<Animator>().Play(deadAnime);
        // 1���� ĳ���� ����
        Destroy(gameObject, 1.0f);

        // BGM ���� �� ���� ���� SE ���
        SoundManager.soundManager.StopBGM();
        SoundManager.soundManager.SEPlay(SEType.GameOver);
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