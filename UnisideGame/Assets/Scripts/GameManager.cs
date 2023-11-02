using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ���ÿ�� ��뿡 �ռ� �ݵ�� �߰�

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;    // �̹����� �����ص� GameObject
    public Sprite gameOverSpr;      // GameOver �̹���
    public Sprite gameClearSpr;     // GameClear �̹���
    public GameObject panel;        // �г�
    public GameObject restartButton;// Restart ��ư
    public GameObject nextButton;   // Next ��ư

    Image titleImage;               // �̹��� ǥ�ø� ����ϴ� Image ������Ʈ

    // �ð����� ��� ���
    public GameObject timeBar;      // �ð� ǥ�� �̹���
    public GameObject timeText;     // �ð� �ؽ�Ʈ
    TimeController timeCnt;         // TimeController Ŭ����

    public GameObject scoreText;    // ���� �ؽ�Ʈ
    public static int totalScore;   // ����
    public int stageScore = 0;      // �������� ����

    public AudioClip gameOverSound; // ���� ���� ����
    public AudioClip gameClearSound;// ���� Ŭ���� ����

    public GameObject InputUI;      // �÷��̾� ���� UI �г�
    // Start is called before the first frame update
    void Start()
    {
        // �̹��� �����
        Invoke("InactiveImage", 1.0f);  //1���� �Լ�����
        // ��ư(�г�) �����
        panel.SetActive(false);

        // �ð����� �߰�
        timeCnt = GetComponent<TimeController>();
        if(timeCnt!=null)
        {
            if(timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);   // �ð������� ���� ��� ���� ó��
            }
        }

        // ���� �߰�
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        // ������ Ŭ���� ���� ���
        if(PlayerController.gameState == "gameclear")
        {
            mainImage.SetActive(true);  //�̹��� ǥ��
            panel.SetActive(true);      //��ư(�г�)ǥ��

            //RESTART ��ư ��ȿȭ
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

            // ���ӽð� ī��Ʈ ����
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;

                // ���� �߰�
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;    //���� �ð��� ������ ���Ѵ�
            }

            // ������ ���� ������������ �׵��� ������ ���Ѵ�
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();  // ���� ����

            // ���� ���
            AudioSource sound = GetComponent<AudioSource>();
            if(sound != null)
            {
                sound.Stop();   //���� BGM ����
                sound.PlayOneShot(gameClearSound);  
            }

            // �÷��̾� ����
            InputUI.SetActive(false);   //�����
        }

        // ���ӿ����� ���
        else if(PlayerController.gameState=="gameover")
        {
            mainImage.SetActive(true);  //�̹���ǥ��
            panel.SetActive(true);      //��ư(�г�) ǥ��

            //NEXTǥ�� ��Ȱ��
            Button bt = nextButton.GetComponent<Button>(); 
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            // ���ӽð� ī��Ʈ ����
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;
            }

            // ���� ���
            AudioSource sound = GetComponent<AudioSource>();
            if (sound != null)
            {
                sound.Stop();   //���� BGM ����
                sound.PlayOneShot(gameOverSound);
            }
            // �÷��̾� ����
            InputUI.SetActive(false);   //�����
        }
        
        // ���� �÷��� ���� ���
        else if (PlayerController.gameState == "playing")
        {
            // ���� Scene���� Player �̸��� �±׸� ���� ������Ʈ�� �����´�
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // PlayerController ��������
            PlayerController playerCnt = player.GetComponent<PlayerController>();

            // �ð� ���� ������Ʈ
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //���������� ����ȯ�� �ǽ��Ͽ� �Ҽ��� ���ϸ� ������
                    int time = (int)timeCnt.displayTime;
                    //�ð� ������Ʈ
                    timeText.GetComponent<Text>().text = time.ToString();
                    //Ÿ�� ����
                    if (time == 0)
                    {
                        playerCnt.GameOver();//���ӿ���
                    }

                    if(playerCnt.score!=0)
                    {
                        stageScore += playerCnt.score;
                        playerCnt.score = 0;
                        UpdateScore();  // ���� ����
                    }
                }
            }
        }              
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //���� �߰�
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }

    // �÷��̾� ���� �Լ�
    public void Jump()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerCnt = player.GetComponent<PlayerController>();
        playerCnt.Jump();
    }
}

