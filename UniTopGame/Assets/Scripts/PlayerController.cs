using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 3.0f;  // 이동속도

    public float jump = 9.0f;    // 점프력
    public LayerMask groundLayer;// 착지 가능한 레이어

    // 애니메이션
    public string upAnime = "PlayerUp";
    public string downAnime = "PlayerDown";
    public string leftAnime = "PlayerLeft";
    public string rightAnime = "PlayerRight";
    public string deadAnime = "PlayerDead";

    string nowAnimation = "";       // 현재 애니메이션
    string oldAnimation = "";       // 이전 애니메이션

    float axisH = 0.0f;         // 가로 입력 (-1.0 ~ 1.0)
    float axisV = 0.0f;         // 세로 입력 (-1.0 ~ 1.0)
    public float angleZ = -90.0f; // 회전

    Rigidbody2D rbody;          // RigidBody 2D 컴포넌트
    bool isMoving = false;      // 이동 중 

    public static int hp = 3;       // 플레이어의 HP
    public static string gameState; // 게임 상태
    bool inDamage = false;          // 피격상태


    // Start is called before the first frame update
    void Start()
    {
        // 햔제 이 스크립트가 등록된 오브젝트의 RigidBody 2D 컴포넌트 정보 가져오기
        rbody = this.GetComponent<Rigidbody2D>();

        // (기본)애니메이션 설정
        oldAnimation = downAnime;

        // 게임 상태 지정
        gameState = "playing";

        // HP 불러오기
        hp = PlayerPrefs.GetInt("PlayerHP");
    }

    // Update is called once per frame
    void Update()
    {
        // 게임 중이 아니거나 공격받고 있을 경우에는 아무것도 하지 않음
        if (gameState != "playing" || inDamage)
        {
            return;
        }

        if(isMoving==false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); //좌우
            axisV = Input.GetAxisRaw("Vertical");   //상하 (-1~1)
        }

        // 키입력을 통하여 이동 각도 구하기
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);
        angleZ = GetAngle(fromPt, toPt);

        // 이동 각도를 바탕으로 방향과 애니메이션을 변경한다
        if(angleZ >= -45&&angleZ<45)
        {
            // 오른쪽
            nowAnimation = rightAnime;
        }
        else if(angleZ>=45&& angleZ<=135)
        {
            // 윗쪽
            nowAnimation = upAnime;
        }
        else if(angleZ>=-135&& angleZ<-45)   
        {
            // 아랫쪽
            nowAnimation = downAnime;
        }
        else 
        {
            // 왼쪽
            nowAnimation = leftAnime;
        }

        // 애니메이션 변경
        if(nowAnimation !=oldAnimation)
        {
            oldAnimation = nowAnimation;
            GetComponent<Animator>().Play(nowAnimation);
        }
    }

    // (초기설정기준) 0.02초마다 호출됨 (1초에 50번)
    // 물리관련처리는 여기서 해야댐
    void FixedUpdate()
    {
        // 게임중이 아니면 아무것도 하지 않음
        if(gameState != "playing")
        {
            return;
        }

        // 공격받는 도중에 캐릭터를 점멸시킨다
        if(inDamage)
        {
            // Time.time : 게임 시작부터 현재까지의 경과시간 (초단위)
            // Sin 함수에 연속적으로 증가하는 값을 대입하면 0~1~0~-1~0... 순으로 변함
            float value = Mathf.Sin(Time.time * 50);
            Debug.Log(value);
            if(value>0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            return; // 피격중 아무고토못하게함
        }

        // 이동 속도를 더하여 캐릭터를 움직여준다
        rbody.velocity = new Vector2(axisH, axisV) * speed;
    }

    // 터치스크린(가상패드)에서 사용할 함수
    public void SetAxis(float h,float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV ==0)
            isMoving = false;
        else
            isMoving = true;
    }

    // p1에서 p2까지의 각도를 계산한다
    float GetAngle(Vector2 p1,Vector2 p2)
    {
        float angle;

        // 축 방향에 관계없이 캐릭터가 움직이고 있을 경우
        if(axisH!=0 || axisV!=0)
        {
            // p1과 p2의 차를 구하기 (원점을 0으로 하기 위해)
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;

            // 아크탄젠트 함수로 각도(라디안) 구하기
            float rad = Mathf.Atan2(dy, dx);

            // 라디안에서 각도로 변환
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            // 캐릭터가 정지 중이면 각도 유지
            angle = angleZ;
        }
        return angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy와 물리적으로 충돌 발생
        if(collision.gameObject.tag == "Enemy")
        {
            // 데미지 계산
            GetDamage(collision.gameObject);
        }
    }

    void GetDamage(GameObject enemy)
    {
        if(gameState == "playing")
        {
            hp--;   //HP감소
            PlayerPrefs.SetInt("PlayerHP", hp); //현재 HP 저장

            if(hp>0)
            {
                // 이동 중지
                rbody.velocity = new Vector2(0, 0);
                // 히트백 (적이 공격한 방향의 반대로)
                Vector3 toPos = (transform.position - enemy.transform.position).normalized;
                rbody.AddForce(new Vector2(toPos.x * 4, toPos.y * 4), ForceMode2D.Impulse);
                // 현재 공격받고 있음
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                // 체력이 없으면 게임오버
                GameOver();
            }
        }
    }

    // 데미지 처리 종료
    void DamageEnd()
    {
        inDamage = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // 게임오버 처리
    void GameOver()
    {
        Debug.Log("게임오버!");
        gameState = "gameover";

        // 게임오버 연출
        // 충돌 판정 비활성화
        GetComponent<CircleCollider2D>().enabled = false;
        // 이동 중지
        rbody.velocity = new Vector2(0, 0);
        // 중력을 통해 플레이어를 위로 살짝 띄우기
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        // 애니메이션 변경
        GetComponent<Animator>().Play(deadAnime);
        // 1초후 캐릭터 삭제
        Destroy(gameObject, 1.0f);

        // BGM 정지 후 게임 오버 SE 출력
        SoundManager.soundManager.StopBGM();
        SoundManager.soundManager.SEPlay(SEType.GameOver);
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