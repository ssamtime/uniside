using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;                      //적 체력
    public float speed = 0.5f;              //이동속도
    public float reactionDistane = 4.0f;    //플레이어와의 반응범위
    public string idleAnime = "EnemyIdle";  //애니메이션 목록
    public string upAnime = "EnemyUp";
    public string downAnime = "EnemyDown";
    public string rightAnime = "EnemyRight";
    public string leftAnime = "EnemyLeft";
    public string deadAnime = "EnemyDead";
    string nowAnimation = "";               //현재&이전애니메이션
    string oldAnimation = "";
    float axisH;                            //입력된 이동 값
    float axisV;
    Rigidbody2D rbody;                      
    bool isActive = false;      
    public int arrangedId = 0;              //배치 식별용 데이터

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (isActive)
            {
                // 플레이어와의 거리를 바탕으로 각도를 구하기
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float rad = Mathf.Atan2(dy, dx);
                float angle = rad * Mathf.Rad2Deg;

                // 산출된 각도에 따른 애니메이션 구분
                if (angle > -45.0f && angle <= 45.0f)
                    nowAnimation = rightAnime;
                else if (angle > 45.0f && angle <= 135.0f)
                    nowAnimation = upAnime;
                else if (angle >= -135.0f && angle <= -45.0f)
                    nowAnimation = downAnime;
                else
                    nowAnimation = leftAnime;

                // 이동 벡터
                axisH = Mathf.Cos(rad) * speed;
                axisV = Mathf.Sin(rad) * speed;
            }
            else
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (dist < reactionDistane)
                {
                    isActive = true;
                }
            }
        }
        else if(isActive)
        {
            isActive = false;
            rbody.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if(isActive && hp>0)
        {
            // 몬스터 이동시키기
            rbody.velocity = new Vector2(axisH, axisV);
            // 애니메이션 변경하기
            if(nowAnimation != oldAnimation)
            {
                oldAnimation = nowAnimation;
                Animator animator = GetComponent<Animator>();
                animator.Play(nowAnimation);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            hp--;
            if (hp <= 0)
            {
                // 사망 판정
                GetComponent<CircleCollider2D>().enabled = false;
                // 몬스터의 이동을 중지한다
                rbody.velocity = new Vector2(0, 0);
                // 사망 애니메이션을 출력한다
                Animator animator = GetComponent<Animator>();
                animator.Play(deadAnime);
                // 애니메이션이 출력되고 0.5초 후에 제거한다
                Destroy(gameObject, 0.5f);

                // 배치 ID 저장
                SaveDataManager.SetArrangeId(arrangedId, gameObject.tag);
            }
        }
    }


}
