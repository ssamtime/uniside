using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenPoint : MonoBehaviour
{
    // ȭ�鿡 �߰��� ������
    public GameObject objPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectCreate()
    {
        // �������� �̿��Ͽ� ������Ʈ �����
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, -1.0f);
        Instantiate(objPrefab, pos, Quaternion.identity);
    }
}