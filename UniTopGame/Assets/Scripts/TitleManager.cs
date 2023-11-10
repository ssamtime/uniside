using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      //게임시작
    public GameObject continueButton;   //이어하기
    public string firstSceneName;       //게임시작 첫화면 이름

    // Start is called before the first frame update
    void Start()
    {
        // 게임을 실행했을 때 마지막으로 저장된 Scene이 있는지
        string sceneName = PlayerPrefs.GetString("LastScene");
        if(sceneName == "")
        {
            // 컨티뉴 버튼을 비활성화
            continueButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            // 저장된 Scene이 있는 경우 컨티뉴 버튼을 활성화
            continueButton.GetComponent<Button>().interactable = true;
        }

        // 타이틀 BGM 재생
        SoundManager.soundManager.PlayBGM(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        // 기존 데이터를 삭제
        PlayerPrefs.DeleteAll();

        // 데이터 초기화
        PlayerPrefs.SetInt("PlayerHP", 3);
        PlayerPrefs.SetString("LastScene", firstSceneName);
        RoomManager.doorNumber = 0;

        // Scene 이동
        SceneManager.LoadScene(firstSceneName);
    }

    public void ContinueButtonClicked()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");
        RoomManager.doorNumber = PlayerPrefs.GetInt("LastDoor");
        SceneManager.LoadScene(sceneName);
    }
}
