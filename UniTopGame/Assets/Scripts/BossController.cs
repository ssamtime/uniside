using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int hp = 10;                   //체력
    public float reactionDistance = 7.0f; //반응 거리

    public GameObject bulletPrefab;       //총알
    public float shootSpeed = 5.0f;       //총알 속도

    bool inAttack = false;                //공격 상태

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp>0)
        {
            // 플레이어 게임오브젝트 가져오기
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player!= null)
            {
                // 플레이어와의 거리 확인
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);

                // 플레이어가 범위 안에 있고 공격 중이 아닌 경우
                if(dist<=reactionDistance && inAttack == false)
                {
                    // 공격 애니메이션 실행
                    inAttack = true;
                    GetComponent<Animator>().Play("BossAttack");
                }
                // 플레이어가 인식 범위를 벗어난 경우
                else if(dist>reactionDistance && inAttack)
                {
                    //대기 애니메이션 실행
                    inAttack = false;
                    GetComponent<Animator>().Play("BossIdle");
                }                
            }
            else
            {
                //대기 애니메이션 실행
                inAttack = false;
                GetComponent<Animator>().Play("BossIdle");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 보스가 화살을 맞았을 경우
        if(collision.gameObject.tag == "Arrow")
        {
            hp--;
            if(hp<=0)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                GetComponent<Animator>().Play("BossDead");
                Destroy(gameObject, 1);
            }
        }
    }

    // 공격
    void Attack()
    {
        // 발사 위치로 사용할 게임오브젝트 가져오기
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        // 플레이어 정보가 확인되었을 경우 총알 발사
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;

            // 아크탄젠트2 함수로 라디안(호도법) 구하기
            float rad = Mathf.Atan2(dy, dx);

            // 라디안을 각도(60분법)로 변환
            float angle = rad * Mathf.Rad2Deg;

            // 프리팹을 이용하여 총알 오브젝트 만들기 (진행 방향으로 회전)
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // 총알 발사
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
