using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 종류
public enum ItemType
{
    arrow,
    key,
    life
}

public class ItemData : MonoBehaviour
{
    public ItemType type;       //아이템 종류
    public int count = 1;       //아이템 수
    public int arrangedId = 0;  //식별용 값

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 접촉한 게임오브젝트 == 플레이어
        if(collision.gameObject.tag == "Player")
        {
            if(type==ItemType.key)
            {
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                if(PlayerController.hp<3)
                {
                    PlayerController.hp++;
                    // 플레이어 HP 저장
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }

            // 아이템 휙득 연출
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            itemBody.gravityScale = 2.5f;
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);

            // 아이템 배치 ID 저장
            SaveDataManager.SetArrangeId(arrangedId, gameObject.tag);
        }
    }
}
