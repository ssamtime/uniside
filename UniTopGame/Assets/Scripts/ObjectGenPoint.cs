using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenPoint : MonoBehaviour
{
    // 화면에 추가할 프리팹
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
        // 프리팹을 이용하여 오브젝트 만들기
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, -1.0f);
        Instantiate(objPrefab, pos, Quaternion.identity);
    }
}
