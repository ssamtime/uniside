using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataList arrangeDataList; //��ġ ������

    // Start is called before the first frame update
    void Start()
    {
        // SaveDataList �ʱ�ȭ
        arrangeDataList = new SaveDataList() { };
        arrangeDataList.saveDatas = new SaveData[] { };

        // Scene �̸� �ҷ�����
        string stageName = PlayerPrefs.GetString("LastScene");
        // Scene �̸��� Key�� �Ͽ� ����� ������ �ҷ�����
        string data = PlayerPrefs.GetString(stageName);
        if(data!= "")
        {
            //------------ ����� �����Ͱ� �����ϴ� ��� ------------//
            // JSON���� SaveDataList�� ��ȯ
            arrangeDataList = JsonUtility.FromJson<SaveDataList>(data);
            // �迭���� �����͸� ��������
            for(int i =0; i<arrangeDataList.saveDatas.Length; i++)
            {
                SaveData savedata = arrangeDataList.saveDatas[i];
                // �±׸� ����Ͽ� ���ӿ�����Ʈ ã��
                string objTag = savedata.objTag;
                GameObject[] objects = GameObject.FindGameObjectsWithTag(objTag);
                
                for(int j=0; j<objects.Length;j++)
                {
                    // �迭���� GameObject ��������
                    GameObject obj = objects[j];

                    // GameObject�� ����� �±� Ȯ��
                    if(objTag == "Door")
                    {
                        Door door = obj.GetComponent<Door>();
                        if(door.arrangedId == savedata.arrangedID)
                        {
                            Destroy(obj);   // arrangedId�� ������ ����
                        }
                    }
                    else if( objTag == "ItemBox")
                    {
                        ItemBox box = obj.GetComponent<ItemBox>();
                        if(box.arrangeId == savedata.arrangedID)
                        {
                            box.isClosed = false;   //arrangeId�� ������ ����
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;
                        }
                    }
                    else if(objTag == "Item")   //���� ������
                    {
                        ItemData item = obj.GetComponent<ItemData>();
                        if(item.arrangedId == savedata.arrangedID)
                        {
                            Destroy(obj);   //arrangeId�� ������ ����
                        }
                    }
                    else if(objTag=="Enemy")
                    {
                        Enemy enemy = obj.GetComponent<Enemy>();
                        if(enemy.arrangedId == savedata.arrangedID)
                        {
                            Destroy(obj);   //arrangeID�� ������ ����
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

    // ��ġ ������ ID ����
    public static void SetArrangeId(int arrangeId,string objTag)
    {
        // ID�� 0�̰� �±׿� �ƹ��͵� �������� ������ ��� ����
        if(arrangeId == 0 || objTag == "")
        {
            return;
        }

        SaveData[] newSaveDatas = new SaveData[arrangeDataList.saveDatas.Length + 1];
        // ������ ����
        for(int i=0; i<arrangeDataList.saveDatas.Length;i++)
        {
            newSaveDatas[i] = arrangeDataList.saveDatas[i];
        }

        // SaveData �����
        SaveData savedata = new SaveData();
        savedata.arrangedID = arrangeId;    //ID ���
        savedata.objTag = objTag;           //�±ױ��

        // SaveData �߰�
        newSaveDatas[arrangeDataList.saveDatas.Length] = savedata;
        arrangeDataList.saveDatas = newSaveDatas;
    }

    // ����� ������ ����
    public static void SaveArrangeData(string stageName)
    {
        if(arrangeDataList.saveDatas != null && stageName !="")
        {
            // SaveDataList�� JSON �����ͷ� ��ȯ
            string saveJson = JsonUtility.ToJson(arrangeDataList);
            // Scene �̸��� Key�ν� ����
            PlayerPrefs.SetString(stageName, saveJson);
        }
    }
}
