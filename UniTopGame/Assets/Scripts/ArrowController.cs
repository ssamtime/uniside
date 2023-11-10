using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // ȭ���� �߻���� ���� �� �����ϴ� �ð�
    public float deletTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        // ���� �ð� ��� �� ����
        Destroy(gameObject, deletTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� ���ӿ�����Ʈ�� �ڽ����� ����
        transform.SetParent(collision.transform);

        // �浹 ���� ��Ȱ��ȭ
        GetComponent<CircleCollider2D>().enabled = false;
        // ���� �ùķ��̼� ��Ȱ��ȭ
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
