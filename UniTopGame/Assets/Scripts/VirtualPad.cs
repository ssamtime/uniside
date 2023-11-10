using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLenght = 70;    //���� �����ϼ� �ִ� �ִ� �Ÿ�
    public bool is4DPad = false;    //�����¿�� �����̴��� �Ǵ�
    GameObject player;              //������ �÷��̾�
    Vector2 defPos;                 //�� �ʱ� ��ǥ
    Vector2 downPos;                //��ġ ��ġ

    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾� ĳ���� ��������
        player = GameObject.FindGameObjectWithTag("Player");
        //�� �ʱ� ��ǥ
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��ġ�ٿ� �̺�Ʈ
    public void PadDown()
    {
        // ���콺 ����Ʈ�� ��ũ�� ��ǥ
        downPos = Input.mousePosition;
    }

    // �巡�� �̺�Ʈ
    public void PadDrag()
    {
        // ���콺 ������
        Vector2 mousePosition = Input.mousePosition;
        // ���ο� �� ��ġ ��ǥ ���ϱ�
        Vector2 newTabPos = mousePosition - downPos;    //���콺 �ٿ� ��ġ������ �̵� �Ÿ�

        if (is4DPad == false)
        {
            newTabPos.y = 0;    //�ݽ�ũ�� �϶��� Y�� ���� 0���� �Ѵ�
        }
        // �̵� ���� ���
        Vector2 axis = newTabPos.normalized;
        // ���� ������ �Ÿ� ���
        float len = Vector2.Distance(defPos, newTabPos);
        if(len>MaxLenght)
        {
            // �Ѱ�ġ�� �Ѿ����Ƿ� �� �̻����� �Ѿ�� �ʵ���
            newTabPos.x = axis.x * MaxLenght;
            newTabPos.y = axis.y * MaxLenght;
        }
        // �� �̹��� �̵�
        GetComponent<RectTransform>().localPosition = newTabPos;
        // �÷��̾� ĳ���� �̵�
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(axis.x, axis.y);
    }

    // �� �̺�Ʈ
    public void PadUp()
    {
        // �� ��ġ �ʱ�ȭ
        GetComponent<RectTransform>().localPosition = defPos;
        // �÷��̾� ĳ���� ����
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(0,0);
    }

    // ���� �̺�Ʈ
    public void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ArrowShoot shoot = player.GetComponent<ArrowShoot>();
        shoot.Attack();
    }
}
