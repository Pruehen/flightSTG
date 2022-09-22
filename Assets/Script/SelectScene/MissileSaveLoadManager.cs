using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

public class MissileSaveLoadManager : MonoBehaviour
{
    public static MissileSaveLoadManager instance;
    private void Awake()
    {        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string SaveDataFileName = "MissileSaveData.json";

    List<SaveData> saveDatas = new List<SaveData>();
    public SaveData GetSaveData(int index)
    {
        if (index >= saveDatas.Count)
            return null;
        return saveDatas[index];
    }

    public bool SerchSaveData(string missileName)
    {
        for(int i = 0; i < saveDatas.Count; i++)
        {
            if (saveDatas[i].name == missileName)
                return true;
        }
        return false;
    }
    /*public SaveData saveData
    {
        get
        {
            if (_saveData != null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _saveData;
        }
    }*/
    private void Start()
    {
        LoadGameData();

        //AddData("Aim-9L", SelectSystem.instance.GetMissileSpec());
        print(GetSaveData(0));
        print(GetSaveData(1));
    }

    public void LoadGameData()
    {
        string filePath = Application.dataPath + SaveDataFileName;
        string FromJsonData = File.ReadAllText(filePath);
        saveDatas = JsonConvert.DeserializeObject<List<SaveData>>(FromJsonData);

        print(FromJsonData);
        Debug.Log("불러오기 성공");
    }

    public TMP_InputField inputField;
    string missileName;
    public void SaveBtnClick()
    {
        missileName = inputField.text;
        AddData(missileName, SelectSystem.instance.GetMissileSpec());
    }
    void AddData(string name_, MissileData data_)
    {
        if (data_ == null)
        {
            Debug.Log(missileName);
            MainManager.instance.SystemMessagePrint("완전한 미사일이 아닙니다.");
        }
        else
        {
            if (SerchSaveData(name_))
            {
                MainManager.instance.SystemMessagePrint("이미 동일한 이름의 미사일이 존재합니다.");
                return;
            }
            if (saveDatas.Count > 5)
            {
                MainManager.instance.SystemMessagePrint("저장된 미사일이 너무 많습니다. 기존 데이터를 삭제해주세요.");
                return;
            }
            saveDatas.Add(new SaveData(name_, DataToFloat(data_)));
            MainManager.instance.SystemMessagePrint("저장되었습니다.");
            SaveGameData();
        }
    }
    public void InsertData(int index, string name_, float[] data_)
    {
        saveDatas.Insert(index, new SaveData(name_, data_));
        SaveGameData();
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonConvert.SerializeObject(saveDatas);
        string filePath = Application.dataPath + SaveDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("저장 완료");

    }    

    void OnApplicationQuit()
    {
        SaveGameData();
    }

    float[] DataToFloat(MissileData missileData)
    {
        float[] floatdata = new float[10];
        floatdata[0] = missileData.lifeTime;
        floatdata[1] = missileData.firstBurnTime;
        floatdata[2] = missileData.secondBurnTime;
        floatdata[3] = missileData.MAX_G;
        floatdata[4] = missileData.enginePower;
        floatdata[5] = missileData.MAX_TURN_RATE;
        floatdata[6] = missileData.MAX_BORESITE;
        floatdata[7] = missileData.defaultDrag;
        floatdata[8] = missileData.seekerType;
        floatdata[9] = missileData.bodyType;
        return floatdata;
    }
    public MissileData FloatToData(float[] lowData)
    {
        MissileData missileData = new MissileData();
        missileData.lifeTime = lowData[0];
        missileData.firstBurnTime = lowData[1];
        missileData.secondBurnTime = lowData[2];
        missileData.MAX_G = lowData[3];
        missileData.enginePower = lowData[4];
        missileData.MAX_TURN_RATE = lowData[5];
        missileData.MAX_BORESITE = lowData[6];
        missileData.defaultDrag = lowData[7];
        missileData.seekerType = (int)lowData[8];
        missileData.bodyType = (int)lowData[9];

        return missileData;
    }
}

public class SaveData
{
    public string name;
    public float[] data = new float[10];

    public SaveData(string name_, float[] data_)
    {
        name = name_;
        data = data_;
    }
}
