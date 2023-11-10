using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int hasKeys = 0;                //화살 수
    int hasArrows = 0;              //열쇠 수
    int hp = 0;                     //hp
    public GameObject arrowText;    //화살 수 표시용 텍스트
    public GameObject keyText;      //열쇠 수 표시용 텍스트
    public GameObject hpImage;      //HP 표시용 이미지
    public Sprite life3Image;       //HP3
    public Sprite life2Image;       //HP2
    public Sprite life1Image;       //HP1
    public Sprite life0Image;       //HP0
    public GameObject mainImage;    //이미지를 가진 GameObject
    public GameObject resetButton;  //재시작 버튼
    public Sprite gameOverSpr;      //게임오버
    public Sprite gameClearSpr;     //게임클리어
    public GameObject inputPanel;   //버추얼 패드 패널

    public string retrySceneName = "";//재시작하려는 scene이름

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemCount();  //아이템 개수 갱신
        UpdateHP();         //HP 갱신

        // 이미지 숨기기
        Invoke("InactiveImage", 1.0f);
        // 버튼 숨기기
        resetButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemCount();  //아이템 갱신
        UpdateHP();         //HP 갱신
    }

    // 아이템 개수 갱신하는 함수
    void UpdateItemCount()
    {
        if(hasArrows != ItemKeeper.hasArrows)
        {
            arrowText.GetComponent<Text>().text = ItemKeeper.hasArrows.ToString();
            hasArrows = ItemKeeper.hasArrows;
        }

        if(hasKeys!=ItemKeeper.hasKeys)
        {
            keyText.GetComponent<Text>().text = ItemKeeper.hasKeys.ToString();
            hasKeys = ItemKeeper.hasKeys;
        }
    }

    // 채력 갱신하는 함수
    void UpdateHP()
    {
        if(PlayerController.gameState != "gameend")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player!= null)
            {
                hp = PlayerController.hp;
                if(hp<=0)
                {
                    // 플레이어 사망 처리
                    hpImage.GetComponent<Image>().sprite = life0Image;
                    resetButton.SetActive(true);
                    mainImage.SetActive(true);
                    mainImage.GetComponent<Image>().sprite = gameOverSpr;
                    inputPanel.SetActive(false);
                    PlayerController.gameState = "gameend";
                }
                else if(hp == 1)
                {
                    hpImage.GetComponent<Image>().sprite = life1Image;
                }
                else if (hp == 2)
                {
                    hpImage.GetComponent<Image>().sprite = life2Image;
                }
                else 
                {
                    hpImage.GetComponent<Image>().sprite = life3Image;
                }
            }
        }
    }

    // 재시작
    public void Retry()
    {
        PlayerPrefs.SetInt("PlayerHP", 3);
        SoundManager.playingBGM = BGMType.None; //각스테이지에 맞는 bgm재생댐
        SceneManager.LoadScene(retrySceneName);

    }

    // 이미지 숨기기
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // 게임 클리어
    public void GameClear()
    {
        //Game Clear 이미지 출력
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr;

        // 조작 UI 숨김 처리
        inputPanel.SetActive(false);

        // 게임 클리어 처리
        PlayerController.gameState = "gameclear";

        // 3초 뒤에 타이틀로 이동
        Invoke("GotoTitle", 3.0f);
    }

    // 타이틀 화면으로 돌아가기
    void GoToTitle()
    {
        PlayerPrefs.DeleteKey("LastScene");
        SceneManager.LoadScene("Title");
    }
}
