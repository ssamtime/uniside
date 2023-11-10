using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;                      //�� ü��
    public float speed = 0.5f;              //�̵��ӵ�
    public float reactionDistane = 4.0f;    //�÷��̾���� ��������
    public string idleAnime = "EnemyIdle";  //�ִϸ��̼� ���
    public string upAnime = "EnemyUp";
    public string downAnime = "EnemyDown";
    public string rightAnime = "EnemyRight";
    public string leftAnime = "EnemyLeft";
    public string deadAnime = "EnemyDead";
    string nowAnimation = "";               //����&�����ִϸ��̼�
    string oldAnimation = "";
    float axisH;                            //�Էµ� �̵� ��
    float axisV;
    Rigidbody2D rbody;                      
    bool isActive = false;      
    public int arrangedId = 0;              //��ġ �ĺ��� ������

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (isActive)
            {
                // �÷��̾���� �Ÿ��� �������� ������ ���ϱ�
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float rad = Mathf.Atan2(dy, dx);
                float angle = rad * Mathf.Rad2Deg;

                // ����� ������ ���� �ִϸ��̼� ����
                if (angle > -45.0f && angle <= 45.0f)
                    nowAnimation = rightAnime;
                else if (angle > 45.0f && angle <= 135.0f)
                    nowAnimation = upAnime;
                else if (angle >= -135.0f && angle <= -45.0f)
                    nowAnimation = downAnime;
                else
                    nowAnimation = leftAnime;

                // �̵� ����
                axisH = Mathf.Cos(rad) * speed;
                axisV = Mathf.Sin(rad) * speed;
            }
            else
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < reactionDistane)
                {
                    isActive = true;
                }
            }
        }
        else if(isActive)
        {
            isActive = false;
            rbody.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if(isActive && hp>0)
        {
            // ���� �̵���Ű��
            rbody.velocity = new Vector2(axisH, axisV);
            // �ִϸ��̼� �����ϱ�
            if(nowAnimation != oldAnimation)
            {
                oldAnimation = nowAnimation;
                Animator animator = GetComponent<Animator>();
                animator.Play(nowAnimation);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            hp--;
            if (hp <= 0)
            {
                // ��� ����
                GetComponent<CircleCollider2D>().enabled = false;
                // ������ �̵��� �����Ѵ�
                rbody.velocity = new Vector2(0, 0);
                // ��� �ִϸ��̼��� ����Ѵ�
                Animator animator = GetComponent<Animator>();
                animator.Play(deadAnime);
                // �ִϸ��̼��� ��µǰ� 0.5�� �Ŀ� �����Ѵ�
                Destroy(gameObject, 0.5f);

                // ��ġ ID ����
                SaveDataManager.SetArrangeId(arrangedId, gameObject.tag);
            }
        }
    }


}
