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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
