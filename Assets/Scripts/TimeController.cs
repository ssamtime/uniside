using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true; // true = 카운트다운으로 시간을 측정
    public float gameTime = 0;      // 게임의 최대시간 (초)
    public bool isTimeOver = false; // true -> 타이머 정지
    public float displayTime = 0;   // 표시 시간 (외부에서 참조)

    float times = 0;                // 현재 시간 (내부)

    // Start is called before the first frame update
    void Start()
    {
        // 카운트 다운
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
            // 시작 시간부터 전체 경과 시간 구하기
            times += Time.deltaTime;

            if(isCountDown)
            {
                // 카운트 다운 처리 시작
                displayTime = gameTime - times;
                if(displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
            else
            {
                // 카운트 업 처리 시작
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
