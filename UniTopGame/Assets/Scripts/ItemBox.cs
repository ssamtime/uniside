using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Sprite openImage;        //상자 오픈 이미지
    public GameObject itemPrefab;   //상자 속에 담긴 아이템
    public bool isClosed = true;    //true=open, false=close
    public int arrangeId = 0;       //배치 식별용 

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
        if(isClosed && collision.gameObject.tag == "Player")
        {
            // 보물상자가 닫혀있는 상태에서 플레이어와 접촉
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false;
            if(itemPrefab!=null)
            {
                // 프리팹을 이용하여 게임오브젝트 생성
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }

            // 배치 데이터 ID 저장
            SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
        }
    }
}
