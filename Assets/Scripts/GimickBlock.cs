using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickBlock : MonoBehaviour
{
    public float length = 0.0f;     //�ڵ� ���� Ž�� �Ÿ�
    public bool isDelete = false;   //���� �� �������� �Ǵ�

    bool isFell = false;            //���� �÷���
    float fadeTime = 0.5f;          //���̵� �ƿ� �ð�

    // Start is called before the first frame update
    void Start()
    {
        // ���� �ùķ��̼� ���� ��������
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ���� Scene���� �˻��Ѵ�
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player!=null)
        {
            // �÷��̾���� �Ÿ��� ���
            float d = Vector2.Distance(transform.position, player.transform.position);
            if(length>=d)
            {
                // ���� ���� ���� ������ ��� ����� ���Ͻ�Ų��
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if(rbody.bodyType == RigidbodyType2D.Static)
                {
                    // ����� ���� �Ӽ��� ����
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }

        // ���� Ʈ���� ON
        if(isFell)
        {
            // ������ �����Ͽ� ���̵�ƿ� ȿ���� ����
            fadeTime -= Time.deltaTime; //���� �����Ӱ��� ���̸�ŭ �ð��� ����
            Color col = GetComponent<SpriteRenderer>().color;   //�÷� ��������
            col.a = fadeTime;   //���� ����
            GetComponent<SpriteRenderer>().color = col; //�÷� �� �缳��
            if(fadeTime<=0.0f)
            {
                // 0���� ������ ������Ʈ�� Scene���� ����
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // �÷��̾�� ������ �� �ִ� ������ Ȯ�εǸ� ����Ʈ���� ON
            if(isDelete)
            {
                isFell = true;
            }

            // isDelete ���� true�� �ٲ�� ������Ʈ�� ȭ�鿡�� �������,
            // ���� �����ϴ� ���������� ���� �߰�
        }
    }
}
