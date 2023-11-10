using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int hasKeys = 0;                //ȭ�� ��
    int hasArrows = 0;              //���� ��
    int hp = 0;                     //hp
    public GameObject arrowText;    //ȭ�� �� ǥ�ÿ� �ؽ�Ʈ
    public GameObject keyText;      //���� �� ǥ�ÿ� �ؽ�Ʈ
    public GameObject hpImage;      //HP ǥ�ÿ� �̹���
    public Sprite life3Image;       //HP3
    public Sprite life2Image;       //HP2
    public Sprite life1Image;       //HP1
    public Sprite life0Image;       //HP0
    public GameObject mainImage;    //�̹����� ���� GameObject
    public GameObject resetButton;  //����� ��ư
    public Sprite gameOverSpr;      //���ӿ���
    public Sprite gameClearSpr;     //����Ŭ����
    public GameObject inputPanel;   //���߾� �е� �г�

    public string retrySceneName = "";//������Ϸ��� scene�̸�

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemCount();  //������ ���� ����
        UpdateHP();         //HP ����

        // �̹��� �����
        Invoke("InactiveImage", 1.0f);
        // ��ư �����
        resetButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemCount();  //������ ����
        UpdateHP();         //HP ����
    }

    // ������ ���� �����ϴ� �Լ�
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

    // ä�� �����ϴ� �Լ�
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
                    // �÷��̾� ��� ó��
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

    // �����
    public void Retry()
    {
        PlayerPrefs.SetInt("PlayerHP", 3);
        SoundManager.playingBGM = BGMType.None; //������������ �´� bgm�����
        SceneManager.LoadScene(retrySceneName);

    }

    // �̹��� �����
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // ���� Ŭ����
    public void GameClear()
    {
        //Game Clear �̹��� ���
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr;

        // ���� UI ���� ó��
        inputPanel.SetActive(false);

        // ���� Ŭ���� ó��
        PlayerController.gameState = "gameclear";

        // 3�� �ڿ� Ÿ��Ʋ�� �̵�
        Invoke("GotoTitle", 3.0f);
    }

    // Ÿ��Ʋ ȭ������ ���ư���
    void GoToTitle()
    {
        PlayerPrefs.DeleteKey("LastScene");
        SceneManager.LoadScene("Title");
    }
}
