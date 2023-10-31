using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    // 오브젝트가 자동으로 삭제되기까지의 시간
    public float deleteTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 3초 뒤에 자동으로 삭제 처리
        Destroy(gameObject, deleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);    //무언가에 닿았을 경우 즉시 제거
    }
}
