using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLenght = 70;    //탭이 움직일수 있는 최대 거리
    public bool is4DPad = false;    //상하좌우로 움직이는지 판단
    GameObject player;              //조작할 플레이어
    Vector2 defPos;                 //탭 초기 좌표
    Vector2 downPos;                //터치 위치

    // Start is called before the first frame update
    void Start()
    {
        //플레이어 캐릭터 가져오기
        player = GameObject.FindGameObjectWithTag("Player");
        //탭 초기 좌표
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 터치다운 이벤트
    public void PadDown()
    {
        // 마우스 포인트의 스크린 좌표
        downPos = Input.mousePosition;
    }

    // 드래그 이벤트
    public void PadDrag()
    {
        // 마우스 포인터
        Vector2 mousePosition = Input.mousePosition;
        // 새로운 탭 위치 좌표 구하기
        Vector2 newTabPos = mousePosition - downPos;    //마우스 다운 위치에서의 이동 거리

        if (is4DPad == false)
        {
            newTabPos.y = 0;    //휭스크롤 일때는 Y축 값을 0으로 한다
        }
        // 이동 벡터 계산
        Vector2 axis = newTabPos.normalized;
        // 두점 사이의 거리 계산
        float len = Vector2.Distance(defPos, newTabPos);
        if(len>MaxLenght)
        {
            // 한계치를 넘었으므로 그 이상으로 넘어가지 않도록
            newTabPos.x = axis.x * MaxLenght;
            newTabPos.y = axis.y * MaxLenght;
        }
        // 탭 이미지 이동
        GetComponent<RectTransform>().localPosition = newTabPos;
        // 플레이어 캐릭터 이동
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(axis.x, axis.y);
    }

    // 업 이벤트
    public void PadUp()
    {
        // 탭 위치 초기화
        GetComponent<RectTransform>().localPosition = defPos;
        // 플레이어 캐릭터 정지
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(0,0);
    }
}
