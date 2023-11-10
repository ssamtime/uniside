using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int hp = 10;                   //ü��
    public float reactionDistance = 7.0f; //���� �Ÿ�

    public GameObject bulletPrefab;       //�Ѿ�
    public float shootSpeed = 5.0f;       //�Ѿ� �ӵ�

    bool inAttack = false;                //���� ����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp>0)
        {
            // �÷��̾� ���ӿ�����Ʈ ��������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player!= null)
            {
                // �÷��̾���� �Ÿ� Ȯ��
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);

                // �÷��̾ ���� �ȿ� �ְ� ���� ���� �ƴ� ���
                if(dist<=reactionDistance && inAttack == false)
                {
                    // ���� �ִϸ��̼� ����
                    inAttack = true;
                    GetComponent<Animator>().Play("BossAttack");
                }
                // �÷��̾ �ν� ������ ��� ���
                else if(dist>reactionDistance && inAttack)
                {
                    //��� �ִϸ��̼� ����
                    inAttack = false;
                    GetComponent<Animator>().Play("BossIdle");
                }                
            }
            else
            {
                //��� �ִϸ��̼� ����
                inAttack = false;
                GetComponent<Animator>().Play("BossIdle");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������ ȭ���� �¾��� ���
        if(collision.gameObject.tag == "Arrow")
        {
            hp--;
            if(hp<=0)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                GetComponent<Animator>().Play("BossDead");
                Destroy(gameObject, 1);
            }
        }
    }

    // ����
    void Attack()
    {
        // �߻� ��ġ�� ����� ���ӿ�����Ʈ ��������
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        // �÷��̾� ������ Ȯ�εǾ��� ��� �Ѿ� �߻�
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;

            // ��ũź��Ʈ2 �Լ��� ����(ȣ����) ���ϱ�
            float rad = Mathf.Atan2(dy, dx);

            // ������ ����(60�й�)�� ��ȯ
            float angle = rad * Mathf.Rad2Deg;

            // �������� �̿��Ͽ� �Ѿ� ������Ʈ ����� (���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // �Ѿ� �߻�
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
