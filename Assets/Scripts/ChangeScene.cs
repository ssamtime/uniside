using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Scene Transition �Ҷ� �ݵ�� �ʿ�

public class ChangeScene : MonoBehaviour
{
    public string sceneName;    // �ҷ��� scene

    void Start()
    {        
    }
    void Update()
    {        
    }

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
