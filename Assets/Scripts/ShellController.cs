using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    // ������Ʈ�� �ڵ����� �����Ǳ������ �ð�
    public float deleteTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 3�� �ڿ� �ڵ����� ���� ó��
        Destroy(gameObject, deleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);    //���𰡿� ����� ��� ��� ����
    }
}
