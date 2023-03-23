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
    //����â���� �ٷ�� ����
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
    //�ݳ����� �ٷ�� ����
    public int combatData { get; private set; }//����������
    public int gold { get; private set; }//���

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
        PlayerPrefs.SetInt("combatData", combatData);//���������� ����
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
        PlayerPrefs.SetInt("gold", gold);//��� ����
        return true;
    }
    //----------------------------------

    public float difficulty { get; private set; }//1 ~ maxStageNum




    public void DifficultySet(float value)
    {
        //Debug.Log(value);
        difficulty = value;
    }

    public int clearStageNum { get; private set; }//��ܰ� ������������ Ŭ�����ߴ���
    
    public void StageClear()
    {
        if(clearStageNum + 1 == difficulty)
        {
            clearStageNum++;
            Debug.Log(clearStageNum);
            PlayerPrefs.SetInt("clearStageNum", clearStageNum);//�������� Ŭ���� ������ ����
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
