using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동속도
    public float speed = 3.0f;
    
    // 애니메이션
    public string upAnime = "PlayerUp";
    public string downAnime = "PlayerDown";
    public string leftAnime = "PlayerLeft";
    public string rightAnime = "PlayerRight";
    public string deadAnime = "PlayerDead";

    // 현재 애니메이션
    string nowAnimation = "";
    // 이전 애니메이션
    string oldAnimation = "";

    float axisH = 0.0f;                     // 가로 입력 (-1.0 ~ 1.0)
    float axisV = 0.0f;                     // 세로 입력 (-1.0 ~ 1.0)
    public float angleZ = -90.0f; // 회전

    Rigidbody2D rbody;              // RigidBody 2D 컴포넌트
    bool isMoving = false;          // 이동 중

    // Start is called before the first frame update
    void Start()
    {
        // 현재 이 스크립트가 등록된 오브젝트의 RigidBody 2D 컴포넌트 정보 가져오기
        rbody = this.GetComponent<Rigidbody2D>();

        // 애니메이터 정보 가져오기
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;

        // 게임 상태 (플레이 중)
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 플레이 중이 아닐 경우에는 처리하지 않도록 한다
        if (gameState != "playing")
            return;

        // 수평 방향 입력
        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }

        // 캐릭터 방향 조절
        if (axisH > 0.0f)
        {
            // 오른쪽 이동
            Debug.Log("오른쪽 이동");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // 왼쪽 이동
            Debug.Log("왼쪽 이동");
            transform.localScale = new Vector2(-1, 1); // 좌우 반전
        }

        // 캐릭터 점프
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // (유니티 초기 설정 기준) 0.02초마다 호출되며, 1초에 총 50번 호출되는 함수
    void FixedUpdate()
    {
        // 게임 플레이 중이 아닐 경우에는 처리하지 않도록 한다
        if (gameState != "playing")
            return;

        // 착지 판정 처리
        onGround = Physics2D.Linecast(transform.position,
                                                                    transform.position - (transform.up * 0.1f),
                                                                    groundLayer);

        if (onGround || axisH != 0)
        {
            // 캐릭터가 지면에 있거나 X축 입력 값이 0이 아닐 경우 velocity 변경
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
        }
        if (onGround && goJump)
        {
            // 캐릭터가 지면에 있을 때 점프 키를 입력했을 경우
            Debug.Log("점프!");
            Vector2 jumpPw = new Vector2(0, jump);                     // 점프를 위한 벡터
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    // 순간적인 힘을 가한다
            goJump = false;                                                                    // 점프 플래그 OFF
        }

        // 애니메이션
        if (onGround)
        {
            // 지면과 맞닿아있을 때
            if (axisH == 0)
                nowAnime = stopAnime;   // 정지
            else
                nowAnime = moveAnime; // 이동
        }
        else
        {
            // 공중에 있을 때
            nowAnime = jumpAnime; // 점프
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // 애니메이션 재생
        }
    }

    // 점프 트리거 ON
    public void Jump()
    {
        goJump = true;  // 점프 플래그 ON
        Debug.Log("점프 키를 입력했음!");
    }

    // 접촉 시작 판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
            Goal(); // 골인
        else if (collision.gameObject.tag == "Dead")
            GameOver(); // 게임오버
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // 점수 아이템 정보 가져오기
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // 점수 얻기
            score = item.value;
            // 아이템 제거
            Destroy(collision.gameObject);
        }
    }

    // 골 지점에 도착했을 때의 애니메이션 재생
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop(); // 게임 중지
    }

    // 게임 오버일 때의 애니메이션 재생
    public void GameOver()
    {
        animator.Play(deadAnime);

        // 게임 오버 처리
        gameState = "gameover";
        GameStop();

        // 게임 오버 연출
        // 플레이어의 충돌 판정을 비활성화
        GetComponent<CapsuleCollider2D>().enabled = false;
        // 플레이어를 살짝 위로 띄워준다
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    // 게임 정지
    void GameStop()
    {
        // rigidBody2D 정보 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        // 속도를 0으로 하여 강제로 멈추게 한다
        rbody.velocity = new Vector2(0, 0);
    }

    // 터치스크린(가상 패드)에서 사용할 함수
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
            isMoving = false;
        else
            isMoving = true;
    }
}

// 키 입력 관련 함수 목록
/*
    // 키보드의 특정 키 입력에 대한 검사
    bool down = Input.GetKeyDown(KeyCode.Space);
    bool press = Input.GetKey(KeyCode.Space);
    bool up = Input.GetKeyUp(KeyCode.Space);

    // 마우스 버튼 입력 및 터치 이벤트에 대한 검사
    // 0 : 마우스 왼쪽 버튼
    // 1 : 마우스 오른쪽 버튼
    // 2 : 마우스 휠 버튼
    bool down = Input.GetMouseButtonDown(0);
    bool press = Input.GetMouseButton(0);
    bool up = Input.GetMouseButtonUp(0);

    // Input Manager에서 설정한 문자열을 기반으로 하는 키 입력 검사
    bool down = Input.GetButtonDown("Jump");
    bool press = Input.GetButton("Jump");
    bool up = Input.GetButtonUp("Jump");

    // 가상의 축에 대한 키 입력 검사
    float axisH = Input.GetAxis("Horizontal");
    float axisV = Input.GetAxisRaw("Vertical");
*/