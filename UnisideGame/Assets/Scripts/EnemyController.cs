using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          //이동속도
    public string direction = "left";   //이동방향
    public float range = 0.0f;          //이동 가능한 범위
    Vector3 defPos;                     //시작 위치

    // Start is called before the first frame update
    void Start()
    {
        if(direction == "right")
        {
            transform.localScale = new Vector2(-1, 1);  //이동방향 변경
        }
        // 시작위치
        defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // range 값이 0이 아니면 현재 위치와 현재 방향에서
        // 반전을 시킬 것인지 판단한다
        if(range > 0.0f)
        {
            // 시작 위치에서 왼쪽으로 range의 반보다 더 이동하였는가?
            if(transform.position.x<defPos.x-(range/2))
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1);
            }
            // 반대의 경우(오른쪽)을 검사한다
            if (transform.position.x>defPos.x+(range/2))
            {
                direction = "left";
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        // Rigidbody2D를 이용하여 적 캐릭터의 이동속도 갱신
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        if(direction=="right")
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 접촉했을 때 방향 바꾸기
        if(direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
