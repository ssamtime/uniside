using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // �� ������ ��ũ�� ����
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    // ���� ��ũ���� �����ϱ� ���� ���꽺ũ��
    public GameObject subScreen;

    // ī�޶��� ���� ������ ���� ���� ��ũ��
    public bool isForceScrollX = false;     // x�� ���� ��ũ�� �÷���
    public float forceScrollSpeedX = 0.5f;  // 1�ʰ� ������ x�� �Ÿ���
    public bool isForceScrollY = false;     // y�� ���� ��ũ�� �÷���
    public float forceScrollSpeedY = 0.5f;  // 1�ʰ� ������ y�� �Ÿ���


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ���� Scene���� �˻��Ѵ�
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            //ī�޶� ��ǥ ����
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;

            // ���� ���� ����ȭ
            if(isForceScrollX)
            {
                //���� ���� ��ũ��
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }
            
            //���ι��� ������Ʈ (�¿� �̵� ����)
            if (x < leftLimit)
                x = leftLimit;
            else if (x > rightLimit)
                x = rightLimit;

            // ���� ���� ����ȭ
            if (isForceScrollY)
            {
                //���� ���� ��ũ��
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }

            //���ι��� ������Ʈ (���� �̵� ����)
            if (y < bottomLimit)
                y = bottomLimit;
            else if (y > topLimit)
                y = topLimit;

            // �÷��̾��� ��ġ�� ������� ī�޶� ���� �� �ֵ��� �Ѵ�
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            // ���� ��ũ�� ��ũ��
            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);
                subScreen.transform.position = v;
            }
        }
    }
}
