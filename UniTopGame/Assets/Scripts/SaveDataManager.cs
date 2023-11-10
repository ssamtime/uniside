using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataList arrangeDataList; //배치 데이터

    // Start is called before the first frame update
    void Start()
    {
        // SaveDataList 초기화
        arrangeDataList = new SaveDataList() { };
        arrangeDataList.saveDatas = new SaveData[] { };

        // Scene 이름 불러오기
        string stageName = PlayerPrefs.GetString("LastScene");
        // Scene 이름을 Key로 하여 저장된 데이터 불러오기
        string data = PlayerPrefs.GetString(stageName);
        if(data!= "")
        {
            //------------ 저장된 데이터가 존재하는 경우 ------------//
            // JSON에서 SaveDataList로 변환
            arrangeDataList = JsonUtility.FromJson<SaveDataList>(data);
            // 배열에서 데이터를 가져오기
            for(int i =0; i<arrangeDataList.saveDatas.Length; i++)
            {
                SaveData savedata = arrangeDataList.saveDatas[i];
                // 태그를 사용하여 게임오브젝트 찾기
                string objTag = savedata.objTag;
                GameObject[] objects = GameObject.FindGameObjectsWithTag(objTag);
                
                for(int j=0; j<objects.Length;j++)
                {
                    // 배열에서 GameObject 가져오기
                    GameObject obj = objects[j];

                    // GameObject에 저장된 태그 확인
                    if(objTag == "Door")
                    {
                        Door door = obj.GetComponent<Door>();
                        if(door.arrangedId == savedata.arrangedID)
                        {
                            Destroy(obj);   // arrangedId가 같으면 제거
                        }
                    }
                    else if( objTag == "ItemBox")
                    {
                        ItemBox box = obj.GetComponent<ItemBox>();
                        if(box.arrangeId == savedata.arrangedID)
                        {
                            box.isClosed = false;   //arrangeId가 같으면 열기
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;
                        }
                    }
                    else if(objTag == "Item")   //각종 아이템
                    {
                        ItemData item = obj.GetComponent<ItemData>();
                        if(item.arrangedId == savedata.arrangedID)
                        {
                            Destroy(obj);   //arrangeId가 같으면 제거
                        }
                    }
                    else if(objTag=="Enemy")
                    {
                        Enemy enemy = obj.GetComponent<Enemy>();
                        if(enemy.arrangedId == savedata.arrangedID)
                        {
                            Destroy(obj);   //arrangeID가 같으면 제거
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 배치 데이터 ID 설정
    public static void SetArrangeId(int arrangeId,string objTag)
    {
        // ID가 0이고 태그에 아무것도 존재하지 않으면 즉시 종료
        if(arrangeId == 0 || objTag == "")
        {
            return;
        }

        SaveData[] newSaveDatas = new SaveData[arrangeDataList.saveDatas.Length + 1];
        // 데이터 복사
        for(int i=0; i<arrangeDataList.saveDatas.Length;i++)
        {
            newSaveDatas[i] = arrangeDataList.saveDatas[i];
        }

        // SaveData 만들기
        SaveData savedata = new SaveData();
        savedata.arrangedID = arrangeId;    //ID 기록
        savedata.objTag = objTag;           //태그기록

        // SaveData 추가
        newSaveDatas[arrangeDataList.saveDatas.Length] = savedata;
        arrangeDataList.saveDatas = newSaveDatas;
    }

    // 기록한 데이터 저장
    public static void SaveArrangeData(string stageName)
    {
        if(arrangeDataList.saveDatas != null && stageName !="")
        {
            // SaveDataList를 JSON 데이터로 변환
            string saveJson = JsonUtility.ToJson(arrangeDataList);
            // Scene 이름을 Key로써 저장
            PlayerPrefs.SetString(stageName, saveJson);
        }
    }
}
