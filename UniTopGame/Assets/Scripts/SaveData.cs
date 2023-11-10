using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int arrangedID = 0;      //배치 ID
    public string objTag = "";      //배치된 오브젝트의 태그
}

[System.Serializable]
public class SaveDataList
{
    public SaveData[] saveDatas;    //savedata 배열
}