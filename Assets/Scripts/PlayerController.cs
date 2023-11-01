using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;          // RigidBody 2D 컴포넌트
    float axisH = 0.0f;         // 좌우 방향키 입력
    public float speed = 3.0f;  // 이동속도

    public float jump = 9.0f;    // 점프력
    public LayerMask groundLayer;// 착지 가능한 레이어
    bool goJump = false;         // 점프 키 입력 상태
    bool onGround = false;       // 지면과 접촉 상태

    // 애니메이터
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    // 게임 상태
    public static string gameState = "playing"; // 함수 벗어나도 변수 유지

    // 점수
    public int score = 0;

    // 터치스크린 조작 관련 변수
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        // 햔제 이 스크립트가 등록된 오브젝트의 RigidBody 2D 컴포넌트 정보 가져오기
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
        // 게임 플레이 중이 아닐 경우 처리하지 않도록 한다
        if (gameState != "playing")
            return;

        // 수평 방향 입력
        if(isMoving ==false)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }
        // 캐릭터 방향 조절
        if(axisH>0.0f)
        {
            // 오른쪽 이동
            Debug.Log("오른쪽 이동");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // 왼쪽 이동
            Debug.Log("왼쪽 이동");
            transform.localScale = new Vector2(-1, 1);  // 좌우 반전
        }

        // 케릭터 점프
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }           
        
    }

    // (초기설정기준) 0.02초마다 호출됨 (1초에 50번)
    // 물리관련처리는 여기서 해야댐
    void FixedUpdate()
    {
        // 게임 플레이 중이 아닐 경우 처리하지 않도록 한다
        if (gameState != "playing")
            return;

        // 착지 판정 처리
        // 시작점, 도착점을 이은 선에 대상 레이어가 접촉하면 true 반환
        onGround = Physics2D.Linecast(transform.position,
                                      transform.position - (transform.up * 0.1f),
                                      groundLayer);

        if(axisH!=0||onGround)
        {
            // 캐릭터가 지면에 있거나, x축 입력 값이 0이 아닐경우  오브젝트의 속도 변경        
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y); // Vector2 (float x, float y)
        }
        if(onGround&&goJump)
        {
            // 캐릭터가 지면에 있을 때 점프 키를 입력했을 경우
            Debug.Log("점프!");
            Vector2 jumpPw = new Vector2(0, jump);       // 점프를 위한 벡터
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); // 순간적인 힘을 가한다
            goJump = false;                              // 점프 플래그 off
        }

        // 애니메이션
        if(onGround)
        {
            // 지면과 맞닿아있을 때 
            if (axisH == 0)
                nowAnime = stopAnime; // 정지
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

    // 점프 트리거 on
    public void Jump()
    {
        goJump = true;  //점프 플래그 on
        Debug.Log("점프 키를 입력했음!");
    }

    // 접촉 시작 판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
            Goal();
        else if (collision.gameObject.tag == "Dead")
            GameOver(); 
        else if(collision.gameObject.tag=="ScoreItem")
        {
            // 점수 아이템 정보 가져오기
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            // 점수 얻기
            score = item.value;
            // 아이템 제거
            Destroy(collision.gameObject);
        }
    }

    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop(); // 게임 중지
    }

    // 게임오버일때 애니메이션 재생
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop();

        // 게임 오버 연출
        // 플레이어의 각종 충돌 판정을 비활성화
        GetComponent<CapsuleCollider2D>().enabled = false;
        // 플레이어를 살짝 위로 이동시킨다
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

    // 터치스크린(가상패드)에서 사용할 함수
    public void SetAxis(float h,float v)
    {
        axisH = h;
        if (axisH == 0)
            isMoving = false;
        else
            isMoving = true;
    }
}


// 키 입력 함수 목록
/*        
bool down = Input.GetKeyDown(KeyCode.Space);
bool press = Input.GetKey(KeyCode.Space);
bool up = Input.GetKeyUp(KeyCode.Space);

// 마우스 버튼 및 터치 이벤트 검사
// 0: 왼쪽 1: 오른쪽 2: 휠버튼
bool down = Input.GetMouseButtonDown(0);
bool press = Input.GetMouseButton(0);
bool up = Input.GetMouseButtonUp(0);

// Input Manager에서 설정한 문자열을 기반으로 하는 키 입력 검사
bool down = Input.GetButtonDown("Jump");
bool press = Input.GetButton("Jump");
bool up = Input.GetButtonUp("Jump");

// 가상의 축에 대한 키 입력
float axisH = Input.GetAxis("Horizontal");  // -1 ~ 1
float axisV = Input.GetAxisRaw("Vertical"); // -1, 0, 1
*/