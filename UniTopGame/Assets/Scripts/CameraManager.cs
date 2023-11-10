using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject otherTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player!= null)
        {
            if(otherTarget!=null)
            {
                // lerp : 보간
                Vector2 pos = Vector2.Lerp(player.transform.position,
                                           otherTarget.transform.position, 0.5f);

                // 플레이어의 좌표와 연동
                transform.position = new Vector3(pos.x, pos.y, -10);
            }
            else
            {
                // 플레이어 위치를 기반으로 카메라 추적을 실시한다
                transform.position = new Vector3(player.transform.position.x,
                                                 player.transform.position.y, -10);
            }
        }
        
    }
}
