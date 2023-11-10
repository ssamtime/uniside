using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      //���ӽ���
    public GameObject continueButton;   //�̾��ϱ�
    public string firstSceneName;       //���ӽ��� ùȭ�� �̸�

    // Start is called before the first frame update
    void Start()
    {
        // ������ �������� �� ���������� ����� Scene�� �ִ���
        string sceneName = PlayerPrefs.GetString("LastScene");
        if(sceneName == "")
        {
            // ��Ƽ�� ��ư�� ��Ȱ��ȭ
            continueButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            // ����� Scene�� �ִ� ��� ��Ƽ�� ��ư�� Ȱ��ȭ
            continueButton.GetComponent<Button>().interactable = true;
        }

        // Ÿ��Ʋ BGM ���
        SoundManager.soundManager.PlayBGM(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        // ���� �����͸� ����
        PlayerPrefs.DeleteAll();

        // ������ �ʱ�ȭ
        PlayerPrefs.SetInt("PlayerHP", 3);
        PlayerPrefs.SetString("LastScene", firstSceneName);
        RoomManager.doorNumber = 0;

        // Scene �̵�
        SceneManager.LoadScene(firstSceneName);
    }

    public void ContinueButtonClicked()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");
        RoomManager.doorNumber = PlayerPrefs.GetInt("LastDoor");
        SceneManager.LoadScene(sceneName);
    }
}
