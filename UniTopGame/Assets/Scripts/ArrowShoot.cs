using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;    //화살 속도
    public float shootDelay = 0.25f;    //발사 간격
    public GameObject bowPrefab;        //활
    public GameObject arrowPrefab;      //화살

    bool inAttack = false;  //공격 상태 판단
    GameObject bowObj;      //활

    // Start is called before the first frame update
    void Start()
    {
        // 활을 플레이어 위치에 배치
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform);  //플레이어 객체를 활 객체의 부모로 설정
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetButtonDown("Fire1")))
        {
            // 공격 키 입력
            Attack();
        }

        // 활 회전과 우선순위 판정
        float bowZ = -1;    //활 객체의 Z축 값 (캐릭터보다 앞에 설정)
        PlayerController plmv = GetComponent<PlayerController>();
        if(plmv.angleZ > 30 && plmv.angleZ <150)
        {
            // 윗방향
            bowZ = 1;
        }
        // 활 회전
        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);
        // 활 우선순위
        bowObj.transform.position = new Vector3(transform.position.x,transform.position.y, bowZ);
    }

    // 공격
    public void Attack()
    {
        // 화살을 가지고 있고 현재 공격 상태가 아닐 경우
        if(ItemKeeper.hasArrows >0 &&inAttack ==false)
        {
            ItemKeeper.hasArrows -= 1;  //화살 1개 소모
            inAttack = true;            //공격 상태 변경

            // 화살 발사
            PlayerController playerCnt = GetComponent<PlayerController>();
            // 회전에 사용할 각도
            float angleZ = playerCnt.angleZ;
            // 화살 오브젝트 생성 (캐릭터 진행 방향으로 회전)
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);

            // 화살을 발사하기 위한 벡터 생성
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // 지정한 각도와 방향으로 화살을 발사
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // 딜레이 설정
            Invoke("StopAttack", shootDelay);

            // SE 재생
            SoundManager.soundManager.SEPlay(SEType.Shoot);
        }
    }

    // 공격 중지
    public void StopAttack()
    {
        inAttack = false;
    }
}
