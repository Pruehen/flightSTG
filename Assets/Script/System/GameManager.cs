using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        PlayerPrefs.Save();
        DontDestroyOnLoad(gameObject);

        BgmSet(1);
        SoundSet(1);

    }
    //설정창에서 다루는 변수
    public float bgmVolume { get; private set; }// 0.001 ~ 1
    public float soundVolume { get; private set; }// 0.001 ~ 1
    public float controlSencitivity { get; private set; } = 3;// 1 ~ 5
    public bool autoAim { get; private set; } = true;

    public bool joyStickUse { get; private set; } = false;

    public void BgmSet(float value)
    {
        bgmVolume = value;
    }
    public void SoundSet(float value)
    {
        soundVolume = value;
    }
    public void ControlSencitivitySet(float value)//value = 2~10
    {
        controlSencitivity = value;
    }
    public void AutoAimSet(bool value)
    {
        autoAim = value;
    }
    public void JoyStickUseSet(bool value)
    {
        joyStickUse = value;
    }
    //----------------------------------
    //격납고에서 다루는 변수
    public int combatData { get; private set; }//전투데이터
    public int gold { get; private set; }//골드

    public void DebugGetData()
    {
        CombatDataUse(-20000);
        GoldUse(-20);
    }

    public bool CombatDataUse(int value)
    {
        if (combatData - value < 0)
            return false;

        combatData -= value;
        Debug.Log(combatData);
        if (HangerWdw.instance != null)
        {
            HangerWdw.instance.CombatDataTextSet();
        }
        PlayerPrefs.SetInt("combatData", combatData);//전투데이터 저장
        return true;
    }
    public bool GoldUse(int value)
    {
        if (gold - value < 0)
            return false;

        gold -= value;
        Debug.Log(gold);
        if (HangerWdw.instance != null)
        {
            HangerWdw.instance.GoldTextSet();
        }
        PlayerPrefs.SetInt("gold", gold);//골드 저장
        return true;
    }
    //----------------------------------

    public float difficulty { get; private set; }//1 ~ maxStageNum




    public void DifficultySet(float value)
    {
        //Debug.Log(value);
        difficulty = value;
    }

    public int clearStageNum { get; private set; }//몇단계 스테이지까지 클리어했는지
    
    public void StageClear()
    {
        if(clearStageNum + 1 == difficulty)
        {
            clearStageNum++;
            Debug.Log(clearStageNum);
            PlayerPrefs.SetInt("clearStageNum", clearStageNum);//스테이지 클리어 데이터 저장
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DataLoad();
        //CombatDataUse(-4000);
        PlayerPrefs.SetInt("combatData", combatData);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(controlSencitivity);
    }

    void DataLoad()
    {
        clearStageNum = PlayerPrefs.GetInt("clearStageNum");
        combatData = PlayerPrefs.GetInt("combatData");
        gold = PlayerPrefs.GetInt("gold");
    }
}
