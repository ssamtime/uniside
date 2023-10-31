using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true; // true = ī��Ʈ�ٿ����� �ð��� ����
    public float gameTime = 0;      // ������ �ִ�ð� (��)
    public bool isTimeOver = false; // true -> Ÿ�̸� ����
    public float displayTime = 0;   // ǥ�� �ð� (�ܺο��� ����)

    float times = 0;                // ���� �ð� (����)

    // Start is called before the first frame update
    void Start()
    {
        // ī��Ʈ �ٿ�
        if(isCountDown)
        {
            displayTime = gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimeOver == false)
        {
            // ���� �ð����� ��ü ��� �ð� ���ϱ�
            times += Time.deltaTime;

            if(isCountDown)
            {
                // ī��Ʈ �ٿ� ó�� ����
                displayTime = gameTime - times;
                if(displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
            else
            {
                // ī��Ʈ �� ó�� ����
                displayTime = times;
                if(displayTime >= gameTime)
                {
                    displayTime = gameTime;
                    isTimeOver = true;
                }
            }
            Debug.Log("TIMES :" + displayTime);
        }
    }
}
