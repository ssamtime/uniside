using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangedId = 0;   //식별용 값

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            if(ItemKeeper.hasKeys > 0)      //열쇠를 가지고 있을 경우
            {
                ItemKeeper.hasKeys--;       //1개 감소
                Destroy(this.gameObject);   //문 오브젝트 제거

                // 아이템 배치 ID 저장
                SaveDataManager.SetArrangeId(arrangedId, gameObject.tag);
            }
        }
    }
}
