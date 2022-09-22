using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        settingScreen.gameObject.SetActive(false);
        missileSaveScreen.gameObject.SetActive(false);
        systemMessage.gameObject.SetActive(false);
        aircraftSet.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick_PLay(int missionNum)
    {
        float[] lowData = { 20.0f, 3.75f, 0.0f, 400.0f, 150.0f, 30.0f, 120.4f, 0.05f, 0.0f, 2.0f };
        MissileData data = MissileSaveLoadManager.instance.FloatToData(lowData);
        StaticMissileData.missileData = data;

        if(StaticMissileData.missileData == null)
        {
            SystemMessagePrint("미사일이 선택되지 않았습니다.");
            return;
        }

        switch(missionNum)
        {
            case 1:
                SceneManager.LoadScene("GamePlayScene_Mission1");
                break;
            case 2:
                SceneManager.LoadScene("GamePlayScene_Mission2");
                break;
            case 3:
                SceneManager.LoadScene("GamePlayScene_Mission3");
                break;
            default:
                break;
        }
    }

    public SelectSystem settingScreen;
    public GameObject missileSaveScreen;
    public SystemMessage systemMessage;
    public AircraftSet aircraftSet;

    public void ButtonClick_Setting(bool value)
    {
        if (value)
        {
            settingScreen.gameObject.SetActive(true);
            settingScreen.SelectScreenInit();
        }
        else
        {
            settingScreen.SelectScreenInit();
            settingScreen.gameObject.SetActive(false);
        }
    }

    public void ButtonClick_MissileSave(bool value)
    {
        if (value)
        {
            missileSaveScreen.gameObject.SetActive(true);
            //MissileSaveLoadManager.SelectScreenInit();
        }
        else
        {
            //MissileSaveLoadManager.SelectScreenInit();
            missileSaveScreen.gameObject.SetActive(false);
        }
    }

    public void ButtonClick_SystemMessage(bool value)
    {
        if (value)
        {
            systemMessage.gameObject.SetActive(true);
            SystemMessagePrint("default_text");
        }
        else
        {
            systemMessage.gameObject.SetActive(false);
        }
    }
    public void SystemMessagePrint(string message)
    {
        systemMessage.gameObject.SetActive(true);
        systemMessage.SystemTextSet(message);
    }

    public void ButtonClick_AircraftSet(bool value)
    {
        if (value)
        {
            aircraftSet.gameObject.SetActive(true);            
        }
        else
        {
            aircraftSet.gameObject.SetActive(false);
        }
    }
}
