using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //scene�̵��� �ʼ�

public class RoomManager : MonoBehaviour
{
    public static int doorNumber = 0; //����ȣ

    // Start is called before the first frame update
    void Start()
    {
        // ���Ա� ������ �迭�� ���޹ޱ�
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");
        for(int i =0; i<enters.Length;i++)
        {
            GameObject doorObj = enters[i];             //�迭�� n��° ��Ҹ� ���޹޾�
            Exit exit = doorObj.GetComponent<Exit>();   //�ش� ���Ա� Exit Ŭ���� ���� �׵�
            if(doorNumber == exit.doorNumber)
            {
                // �÷��̾� ĳ���͸� ���Ա��� �̵�
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;

                if (exit.direction == ExitDirection.up)
                    y += 1;
                else if (exit.direction == ExitDirection.right)
                    x += 1;
                else if (exit.direction == ExitDirection.down)
                    y -= 1;
                else if (exit.direction == ExitDirection.left)
                    x-= 1;

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(x, y);
                break;
            }
        }

        // Scene �̸� ��������
        string sceneName = PlayerPrefs.GetString("LastScene");
        if (sceneName == "BossStage")
            SoundManager.soundManager.PlayBGM(BGMType.InBoss);
        else
            SoundManager.soundManager.PlayBGM(BGMType.InGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Scene �̵�
    public static void ChangeScene(string sceneName, int doorNum)
    {
        doorNumber = doorNum;   //�� ��ȣ�� static ������ ����

        string nowScene = PlayerPrefs.GetString("LastScene");
        if(nowScene!="")
        {
            SaveDataManager.SaveArrangeData(nowScene);  //��ġ��������
        }
        PlayerPrefs.SetString("LastScene", sceneName);  //Scene �̸� ����
        PlayerPrefs.SetInt("LastDoor", doorNum);        //�� ��ȣ ����
        ItemKeeper.SaveItem();      //������ ����

        SceneManager.LoadScene(sceneName);  //�̵� �ǽ�
    }
}

/*
    
    
 */