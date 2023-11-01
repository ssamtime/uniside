using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          //�̵��ӵ�
    public string direction = "left";   //�̵�����
    public float range = 0.0f;          //�̵� ������ ����
    Vector3 defPos;                     //���� ��ġ

    // Start is called before the first frame update
    void Start()
    {
        if(direction == "right")
        {
            transform.localScale = new Vector2(-1, 1);  //�̵����� ����
        }
        // ������ġ
        defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // range ���� 0�� �ƴϸ� ���� ��ġ�� ���� ���⿡��
        // ������ ��ų ������ �Ǵ��Ѵ�
        if(range > 0.0f)
        {
            // ���� ��ġ���� �������� range�� �ݺ��� �� �̵��Ͽ��°�?
            if(transform.position.x<defPos.x-(range/2))
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1);
            }
            // �ݴ��� ���(������)�� �˻��Ѵ�
            if (transform.position.x>defPos.x+(range/2))
            {
                direction = "left";
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody2D�� �̿��Ͽ� �� ĳ������ �̵��ӵ� ����
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        if(direction=="right")
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������� �� ���� �ٲٱ�
        if(direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
