using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;    //��ź ������
    public float delayTime = 3.0f;  //�����ð�
    public float fireSpeedX = -4.0f;//�߻�ӵ� X��
    public float fireSpeedY = 0.0f; //�߻�ӵ� Y��
    public float length = 8.0f;     //

    GameObject player;      //�÷��̾�
    GameObject gateObj;     //����
    float passedTime = 0;   //����ð�

    // Start is called before the first frame update
    void Start()
    {
        // ������ ��ġ�� ������Ʈ ��������
        Transform tr = transform.Find("gate");
        gateObj = tr.gameObject;
        //�÷��̾� ��������
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // �߻� �ð�
        passedTime += Time.deltaTime;

        // �Ÿ� Ȯ��
        if(CheckLength(player.transform.position))
        {
            if(passedTime>delayTime)
            {
                // �߻�
                passedTime = 0;
                // �߻� ��ġ
                Vector3 pos = new Vector3(gateObj.transform.position.x,
                                          gateObj.transform.position.y,
                                          transform.position.z);
                // Prefab���� GameObject �����
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);//Quaternion.Euler(0.0f,0.0f,45.0f)
                // �߻� ����
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }

    // �Ÿ� Ȯ��
    bool CheckLength(Vector2 targetPos)
    {
        bool ret =false;
        float d = Vector2.Distance(transform.position,targetPos);
        if(length>=d)
        {
            ret = true;
        }
        return ret;
    }
}
