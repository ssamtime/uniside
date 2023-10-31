using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          //X축 이동거리
    public float moveY = 0.0f;          //Y축 이동거리
    public float times = 0.0f;          //시간
    public float weight = 0.0f;         //정지
    public bool isMoveWhenOn = false;   //올라탔을 때 이동

    public bool isCanMove = true;       //움직임
    float perDX;                        //프레임당 X축 이동 값
    float perDY;                        //프레임당 Y축 이동 값
    Vector3 defPos;                     //초기위치
    bool isReverse = false;             //방향반전

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
