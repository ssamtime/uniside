using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public enum ItemType
{
    arrow,
    key,
    life
}

public class ItemData : MonoBehaviour
{
    public ItemType type;       //������ ����
    public int count = 1;       //������ ��
    public int arrangedId = 0;  //�ĺ��� ��

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
        // ������ ���ӿ�����Ʈ == �÷��̾�
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
                    // �÷��̾� HP ����
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }

            // ������ �׵� ����
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            itemBody.gravityScale = 2.5f;
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);

            // ������ ��ġ ID ����
            SaveDataManager.SetArrangeId(arrangedId, gameObject.tag);
        }
    }
}
