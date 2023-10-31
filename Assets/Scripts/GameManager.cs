using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련요소 사용에 앞서 반드시 추가

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;    // 이미지를 저장해둘 GameObject
    public Sprite gameOverSpr;      // GameOver 이미지
    public Sprite gameClearSpr;     // GameClear 이미지
    public GameObject panel;        // 패널
    public GameObject restartButton;// Restart 버튼
    public GameObject nextButton;   // Next 버튼

    Image titleImage;               // 이미지 표시를 담당하는 Image 컴포넌트

    // 시간제한 요소 목록
    public GameObject timeBar;      // 시간 표시 이미지
    public GameObject timeText;     // 시간 텍스트
    TimeController timeCnt;         // TimeController 클래스

    public GameObject scoreText;    // 점수 텍스트
    public static int totalScore;   // 총점
    public int stageScore = 0;      // 스테이지 점수

    // Start is called before the first frame update
    void Start()
    {
        // 이미지 숨기기
        Invoke("InactiveImage", 1.0f);  //1초후 함수실행
        // 버튼(패널) 숨기기
        panel.SetActive(false);

        // 시간제한 추가
        timeCnt = GetComponent<TimeController>();
        if(timeCnt!=null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);   // 시간제한이 없을 경우 숨김 처리
            }
        }

        // 점수 추가
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        // 게임을 클리어 했을 경우
        if(PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);  //이미지 표시
            panel.SetActive(true);      //버튼(패널)표시

            //RESTART 버튼 무효화
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

            // 게임시간 카운트 중지
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;

                // 점수 추가
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;    //남은 시간을 점수에 더한다
            }

            // 총점에 현재 스테이지에서 휙득한 점수를 더한다
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();  // 점수 갱신
        }

        //// 게임시간 카운트 중지
        //if(timeCnt!=null)
        //{
        //    timeCnt.isTimeOver = true;
        //}

        // 게임오버일 경우
        else if(PlayerController.gameState=="gameover")
        {
            mainImage.SetActive(true);  //이미지표시
            panel.SetActive(true);      //버튼(패널) 표시

            //NEXT표시 비활성
            Button bt = nextButton.GetComponent<Button>(); 
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            
        }
        
        // 게임 플레이 중일 경우
        else if (PlayerController.gameState == "playing")
        {
            // 현재 Scene에서 Player 이름의 태그를 가진 오브젝트를 가져온다
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // PlayerController 가져오기
            PlayerController playerCnt = player.GetComponent<PlayerController>();

            // 시간 정보 업데이트
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //정수형으로 형변환을 실시하여 소수점 이하를 버린다
                    int time = (int)timeCnt.displayTime;
                    //시간 업데이트
                    timeText.GetComponent<Text>().text = time.ToString();
                    //타임 오버
                    if (time == 0)
                    {
                        playerCnt.GameOver();//게임오버
                    }

                    if(playerCnt.score!=0)
                    {
                        stageScore += playerCnt.score;
                        playerCnt.score = 0;
                        UpdateScore();  // 점수 갱신
                    }
                }
            }
        }              
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //점수 추가
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}

