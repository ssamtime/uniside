using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenManager : MonoBehaviour
{
    // Scene�� ��ġ�� ObjectGenPoint�� �迭
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
                return; //ȭ���� ���������� �Լ� ó���� �����Ѵ�
            }
        }
        
        // �÷��̾��� ���������� ȭ���� ���� Ȯ��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(ItemKeeper.hasArrows ==0 &&player!=null)
        {
            // ȭ���� ������ �迭�� ũ�⸦ �������� ���� ����
            int index = Random.Range(0, objGens.Length);
            ObjectGenPoint objgen = objGens[index];
            objgen.ObjectCreate();  //������ ��ġ
        }
    }
}
