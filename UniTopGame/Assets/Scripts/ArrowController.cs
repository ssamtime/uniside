using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // 화살이 발사된후 게임 상에 존재하는 시간
    public float deletTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        // 일정 시간 경과 후 제거
        Destroy(gameObject, deletTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 게임오브젝트의 자식으로 설정
        transform.SetParent(collision.transform);

        // 충돌 판정 비활성화
        GetComponent<CircleCollider2D>().enabled = false;
        // 물리 시뮬레이션 비활성화
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
