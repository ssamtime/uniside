using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Scene Transition 할때 반드시 필요

public class ChangeScene : MonoBehaviour
{
    public string sceneName;    // 불러올 scene

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
