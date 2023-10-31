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
        // 초기 위치
        defPos = transform.position;
        // 1프레임당 각 축이동에 더할 시간 값
        float timestep = Time.fixedDeltaTime;
        // 1프레임당 x축 이동 값
        perDX = moveX / (1.0f / timestep * times);
        // 1프레임당 y축 이동 값
        perDY = moveY / (1.0f / timestep * times);

        if (isMoveWhenOn)
        {
            //처음에는 움직x 올라타면 움직임
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(isCanMove)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            bool endX = false;
            bool endY = false;
            if(isReverse)
            {
                // 방향 반전
                // 이동 값이 양수고 이동 위치값이 초기 위치값보다 작거나,
                // 이동 값이 음수고 이동 위치값이 초기 위치값보다 큰 경우
                if((perDX>=0.0f&&x<=defPos.x)||(perDX<0.0f&&x>=defPos.x))
                {
                    // 이동 값이 양수
                    endX = true;    //X축 방향 이동 종료
                }
                if ((perDY >= 0.0f && x <= defPos.y) || (perDY < 0.0f && x >= defPos.y))
                {
                    endY = true;    //Y축 방향 이동 종료
                }
                // 블록 이동
                transform.Translate(new Vector3(-perDX, -perDY, defPos.z));
            }
            else
            {
                //정방향 이동 처리
                // 이동 값이 양수고 이동 위치값이 초기 위치값보다 크거나,
                // 이동 값이 음수고 이동 위치값이 (초기 위치값+이동거리)보다 작은 경우
                if ((perDX >= 0.0f && x >= defPos.x+moveX) || (perDX < 0.0f && x <= defPos.x+moveX))
                {
                    // 이동 값이 양수
                    endX = true;    //X축 방향 이동 종료
                }
                if ((perDY >= 0.0f && y >= defPos.y+moveY) || (perDY < 0.0f && y<= defPos.y + moveY))
                {
                    endY = true;    //Y축 방향 이동 종료
                }
                // 블록 이동
                Vector3 v = new Vector3(perDX, perDY, defPos.z);
                transform.Translate(v);
            }

            if(endX&&endY)
            {
                if(isReverse)
                {
                    // 위치 어긋나기 방지용 (정면 방향 이동으로 돌아가기 전 초기 위치로 되돌리기)
                    transform.position = defPos;
                }
                isReverse = !isReverse;     //현재 값 반전
                isCanMove = false;          //이동 가능 트리거 OFF
                // 올라탔을 때의 이동 트리거가 OFF일 경우
                if(isMoveWhenOn==false)
                {
                    // weight값만큼 지연시킨 후 다시 이동 처리
                    Invoke("Move", weight);
                }
            }
        }
    }

    // 이동시키기
    public void Move()
    {
        isCanMove = true;
    }

    // 이동하지 못하게 하기
    public void Stop()
    {
        isCanMove = false;
    }

    // 블록 접촉 1
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // 접촉한 것이 플레이어일 경우  이동하는 블록의 자식 객체로 만들기
            collision.transform.SetParent(transform);
            if(isMoveWhenOn)
            {
                isCanMove = true;
            }
        }
    }

    // 블록 접촉 2
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            // 접촉한 것이 플레이어일 경우 이동하는 블록의 자식 객체에서 제외한다
            collision.transform.SetParent(null);
        }
    }
}
