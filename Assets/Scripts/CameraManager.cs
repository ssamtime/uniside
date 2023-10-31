using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 각 방향의 스크롤 제한
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    // 다중 스크롤을 적용하기 위한 서브스크린
    public GameObject subScreen;

    // 카메라의 동적 연출을 위한 강제 스크롤
    public bool isForceScrollX = false;     // x축 강제 스크롤 플래그
    public float forceScrollSpeedX = 0.5f;  // 1초간 움직일 x축 거리값
    public bool isForceScrollY = false;     // y축 강제 스크롤 플래그
    public float forceScrollSpeedY = 0.5f;  // 1초간 움직일 y축 거리값


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어를 현재 Scene에서 검색한다
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            //카메라 좌표 갱신
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;

            // 가로 방향 동기화
            if(isForceScrollX)
            {
                //가로 강제 스크롤
                x = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }
            
            //가로방향 업데이트 (좌우 이동 제한)
            if (x < leftLimit)
                x = leftLimit;
            else if (x > rightLimit)
                x = rightLimit;

            // 세로 방향 동기화
            if (isForceScrollY)
            {
                //가로 강제 스크롤
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }

            //세로방향 업데이트 (상하 이동 제한)
            if (y < bottomLimit)
                y = bottomLimit;
            else if (y > topLimit)
                y = topLimit;

            // 플레이어의 위치를 기반으로 카메라가 따라갈 수 있도록 한다
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            // 서브 스크린 스크롤
            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);
                subScreen.transform.position = v;
            }
        }
    }
}
