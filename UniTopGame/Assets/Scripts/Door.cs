using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangedId = 0;   //�ĺ��� ��

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
            if(ItemKeeper.hasKeys > 0)      //���踦 ������ ���� ���
            {
                ItemKeeper.hasKeys--;       //1�� ����
                Destroy(this.gameObject);   //�� ������Ʈ ����

                // ������ ��ġ ID ����
                SaveDataManager.SetArrangeId(arrangedId, gameObject.tag);
            }
        }
    }
}
