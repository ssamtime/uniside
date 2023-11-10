using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenManager : MonoBehaviour
{
    // Scene에 배치된 ObjectGenPoint의 배열
    ObjectGenPoint[] objGens;

    // Start is called before the first frame update
    void Start()
    {
        objGens = GameObject.FindObjectsOfType<ObjectGenPoint>();    
    }

    // Update is called once per frame
    void Update()
    {
        ItemData[] items = GameObject.FindObjectsOfType<ItemData>();
        for(int i =0; i<items.Length;i++)
        {
            ItemData item = items[i];
            if (item.type == ItemType.arrow)
            {
                return; //화살이 남아있으면 함수 처리를 종료한다
            }
        }
        
        // 플레이어의 존재유무와 화살의 개수 확인
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(ItemKeeper.hasArrows ==0 &&player!=null)
        {
            // 화살이 없으면 배열의 크기를 기준으로 랜덤 실행
            int index = Random.Range(0, objGens.Length);
            ObjectGenPoint objgen = objGens[index];
            objgen.ObjectCreate();  //아이템 배치
        }
    }
}
