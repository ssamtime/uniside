using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          //X�� �̵��Ÿ�
    public float moveY = 0.0f;          //Y�� �̵��Ÿ�
    public float times = 0.0f;          //�ð�
    public float weight = 0.0f;         //����
    public bool isMoveWhenOn = false;   //�ö����� �� �̵�
    public bool isCanMove = true;       //������

    float perDX;                        //�����Ӵ� X�� �̵� ��
    float perDY;                        //�����Ӵ� Y�� �̵� ��
    Vector3 defPos;                     //�ʱ���ġ
    bool isReverse = false;             //�������

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ� ��ġ
        defPos = transform.position;
        // 1�����Ӵ� �� ���̵��� ���� �ð� ��
        float timestep = Time.fixedDeltaTime;
        // 1�����Ӵ� x�� �̵� ��
        perDX = moveX / (1.0f / timestep * times);
        // 1�����Ӵ� y�� �̵� ��
        perDY = moveY / (1.0f / timestep * times);

        if (isMoveWhenOn)
        {
            //ó������ ����x �ö�Ÿ�� ������
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(isCanMove)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            bool endX = false;
            bool endY = false;
            if(isReverse)
            {
                // ���� ����
                // �̵� ���� ����� �̵� ��ġ���� �ʱ� ��ġ������ �۰ų�,
                // �̵� ���� ������ �̵� ��ġ���� �ʱ� ��ġ������ ū ���
                if((perDX>=0.0f&&x<=defPos.x)||(perDX<0.0f&&x>=defPos.x))
                {
                    // �̵� ���� ���
                    endX = true;    //X�� ���� �̵� ����
                }
                if ((perDY >= 0.0f && y <= defPos.y) || (perDY < 0.0f && y >= defPos.y))
                {
                    endY = true;    //Y�� ���� �̵� ����
                }
                // ��� �̵�
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z));
            }
            else
            {
                //������ �̵� ó��
                // �̵� ���� ����� �̵� ��ġ���� �ʱ� ��ġ������ ũ�ų�,
                // �̵� ���� ������ �̵� ��ġ���� (�ʱ� ��ġ��+�̵��Ÿ�)���� ���� ���
                if ((perDX >= 0.0f && x >= defPos.x+moveX) 
                    || (perDX < 0.0f && x <= defPos.x+moveX))
                {
                    // �̵� ���� ���
                    endX = true;    //X�� ���� �̵� ����
                }
                if ((perDY >= 0.0f && y >= defPos.y+moveY) 
                    || (perDY < 0.0f && y<= defPos.y + moveY))
                {
                    endY = true;    //Y�� ���� �̵� ����
                }
                // ��� �̵�
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if(endX&&endY)
            {
                if(isReverse)
                {
                    // ��ġ ��߳��� ������ (���� ���� �̵����� ���ư��� �� �ʱ� ��ġ�� �ǵ�����)
                    transform.position = defPos;
                }
                isReverse = !isReverse;     //���� �� ����
                isCanMove = false;          //�̵� ���� Ʈ���� OFF
                // �ö����� ���� �̵� Ʈ���Ű� OFF�� ���
                if(isMoveWhenOn==false)
                {
                    // weight����ŭ ������Ų �� �ٽ� �̵� ó��
                    Invoke("Move", weight);
                }
            }
        }
    }

    // �̵���Ű��
    public void Move()
    {
        isCanMove = true;
    }

    // �̵����� ���ϰ� �ϱ�
    public void Stop()
    {
        isCanMove = false;
    }

    // ��� ���� 1
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // ������ ���� �÷��̾��� ���  �̵��ϴ� ����� �ڽ� ��ü�� �����
            collision.transform.SetParent(transform);
            if(isMoveWhenOn)
            {
                isCanMove = true;
            }
        }
    }

    // ��� ���� 2
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            // ������ ���� �÷��̾��� ��� �̵��ϴ� ����� �ڽ� ��ü���� �����Ѵ�
            collision.transform.SetParent(null);
        }
    }
}
