using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimickBlock : MonoBehaviour
{
    public float length = 0.0f;     //자동 낙하 탐지 거리
    public bool isDelete = false;   //낙하 후 제거할지 판단

    bool isFell = false;            //낙하 플래그
    float fadeTime = 0.5f;          //페이드 아웃 시간

    // Start is called before the first frame update
    void Start()
    {
        // 물리 시뮬레이션 정보 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어를 현재 Scene에서 검색한다
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player!=null)
        {
            // 플레이어와의 거리를 계산
            float d = Vector2.Distance(transform.position, player.transform.position);
            if(length>=d)
            {
                // 판정 범위 내에 들어왔을 경우 블록을 낙하시킨다
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if(rbody.bodyType == RigidbodyType2D.Static)
                {
                    // 블록의 물리 속성을 변경
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }

        // 낙하 트리거 ON
        if(isFell)
        {
            // 투명도를 조절하여 페이드아웃 효과를 연출
            fadeTime -= Time.deltaTime; //이전 프레임과의 차이만큼 시간을 차감
            Color col = GetComponent<SpriteRenderer>().color;   //컬러 가져오기
            col.a = fadeTime;   //투명도 변경
            GetComponent<SpriteRenderer>().color = col; //컬러 값 재설정
            if(fadeTime<=0.0f)
            {
                // 0보다 작으면 오브젝트를 Scene에서 삭제
                Destroy(gameObject);
            }
        }

        //void OnCollisionEnter2D(Collision2D collision)
        //{
        //    // 플레이어와 접촉할 수 있는 범위가 확인되면 낙하트리거 ON
        //    if(isDelete)
        //    {
        //        isFell = true;
        //    }

        //    // isDelete 값을 true로 바꿔야 오브젝트가 화면에서 사라지니,
        //    // 각자 제작하는 게임컨셉에 맞춰 추가
        //}
    }
}
