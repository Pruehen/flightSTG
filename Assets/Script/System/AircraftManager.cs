using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftManager : MonoBehaviour
{
    public static AircraftManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public Aircraft useStaticData;
    //string useAircraftName = "C-16C";
    public MissileData[] useMissileDatas;//전투기에 장착될 미사일
    public MissileData UseMissileData(int index)
    {
        return useMissileDatas[index];
    }

    public int indexNum = 0;//어느칸에 미사일을 장착할지 정하는 변수

    public void MissileSet(MissileData missileData)
    {
        useMissileDatas[indexNum] = missileData;
    }

    public void IndexNumSet(int value)
    {
        indexNum = value;
    }

    List<Aircraft> aircraftDatas = new List<Aircraft>();
    List<AircraftUpgradeData> aircraftUpgradeDatas = new List<AircraftUpgradeData>();

    // Start is called before the first frame update
    void Start()
    {
        aircraftDatas.Add(new Aircraft("C-15C", 0.03f, 330, 16, 150, 300, 10, 30, 500, 20, 10, 5));//기체들 데이터 로딩 + 업그레이드데이터 로딩
        aircraftDatas.Add(new Aircraft("C-16A", 0.03f, 230, 12, 120, 300, 10, 25, 500, 15, 10, 5));

        if (useMissileDatas == null)
        {
            useMissileDatas = new MissileData[] { WeaponDatas.instance.datas[0], WeaponDatas.instance.datas[0] };
        }

        UseStaticDataSet(PlayerPrefs.GetString("UseAircraftName", "C-16A"));
        //PlayerPrefs.SetString("UseAircraftName", useStaticData.name);

        //UpgradeResetDebug();//        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Aircraft FindAircraft(string name)
    {
        foreach (Aircraft aircraft in aircraftDatas)
        {
            if (aircraft.name == name)
                return aircraft;
        }
        return null;
    }
    public void UseStaticDataSet(string name)//버튼 누름
    {
        useStaticData = FindAircraft(name);//할당
        PlayerPrefs.SetString("UseAircraftName", name);//저장

        MainSceneManager.Instance.HangerAircraftSet(name);
    }


    public void UpgradeResetDebug()
    {
        useStaticData.AircraftUpgradeData.UpgradeReset();
    }
}

public enum UpgradeType
{
    engine,
    lightness,
    armor,
    reload
}
public class AircraftUpgradeData
{
    public string name { get; private set; }

    public int[] upgradeValue = new int[4];
    public int baseCost { get; private set; }

    public AircraftUpgradeData (string name, int cost)
    {
        this.name = name;
        this.upgradeValue[0] = PlayerPrefs.GetInt(name + "_engineValue", 1);
        this.upgradeValue[1] = PlayerPrefs.GetInt(name + "_lightnessValue", 1);
        this.upgradeValue[2] = PlayerPrefs.GetInt(name + "_armorValue", 1);
        this.upgradeValue[3] = PlayerPrefs.GetInt(name + "_reloadValue", 1);
        baseCost = cost;
    }

    public float EngineValue()
    {
        return upgradeValue[0] * 0.1f + 1;
    }
    public float LightnessValue()
    {
        return upgradeValue[1] * 0.1f + 1;
    }
    public float ArmorValue()
    {
        return upgradeValue[2] * 0.1f + 1;
    }
    public float ReloadValue()
    {
        return upgradeValue[3] * 0.1f + 1;
    }

    public bool TryUpgrade(UpgradeType type)
    {
        if (upgradeValue[(int)type] >= 5)//업그레이드 최대치인경우
            return false;

        int upgradeCost = CostReturn(type);

        if (upgradeCost > 1000)//업그레이드에 소모하는 재화 종류가 전투데이터일경우
        {
            if(GameManager.instance.combatData > upgradeCost)
            {
                UpgradeValueUp((int)type);
                GameManager.instance.CombatDataUse(upgradeCost);
                return true;
            }
            else
            {
                return false;
            }
        }
        else//업그레이드에 소모하는 재화 종류가 골드일 경우
        {
            if(GameManager.instance.gold > upgradeCost)
            {
                UpgradeValueUp((int)type);
                GameManager.instance.GoldUse(upgradeCost);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void UpgradeValueUp(int index)
    {
        upgradeValue[index]++;

        PlayerPrefs.SetInt(name + "_engineValue", this.upgradeValue[0]);
        PlayerPrefs.SetInt(name + "_lightnessValue", this.upgradeValue[1]);
        PlayerPrefs.SetInt(name + "_armorValue", this.upgradeValue[2]);
        PlayerPrefs.SetInt(name + "_reloadValue", this.upgradeValue[3]);
    }

    public int CostReturn(UpgradeType type)
    {
        int dataCost = upgradeValue[(int)type] * 1000 * baseCost;
        int goldCost = (upgradeValue[(int)type]-3) * baseCost;

        if(upgradeValue[(int)type] >= 4)
        {
            return goldCost;
        }
        else
        {
            return dataCost;
        }
    }

    public void UpgradeReset()
    {
        upgradeValue[0] = 1;
        upgradeValue[1] = 1;
        upgradeValue[2] = 1;
        upgradeValue[3] = 1;
        PlayerPrefs.SetInt(name + "_engineValue", 1);
        PlayerPrefs.SetInt(name + "_lightnessValue", 1);
        PlayerPrefs.SetInt(name + "_armorValue", 1);
        PlayerPrefs.SetInt(name + "_reloadValue", 1);
    }
}
