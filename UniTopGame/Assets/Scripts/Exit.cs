using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 출입구 위치 표시
public enum ExitDirection
{
    right,
    left,
    down,
    up
}

public class Exit : MonoBehaviour
{
    public string sceneName = "";   //이동할 scene 이름
    public int doorNumber = 0;      //문 번호
    public ExitDirection direction = ExitDirection.down;    //문 위치

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(doorNumber == 100)
            {
                // 현재 재생중인 BGM 정지
                SoundManager.soundManager.StopBGM();

                // SE 재생(게임클리어)
                SoundManager.soundManager.SEPlay(SEType.GameClear);

                // 게임 클리어
                GameObject.FindObjectOfType<UIManager>().GameClear();
            }
            else
            {
                string nowScene = PlayerPrefs.GetString("LastScene");
                SaveDataManager.SaveArrangeData(nowScene);  //배치 데이터 저장
                RoomManager.ChangeScene(sceneName, doorNumber);
            }           
        }
    }
}
