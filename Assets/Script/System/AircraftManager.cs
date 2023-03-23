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

    public Aircraft useStaticData;//사용하는 데이터
    public Aircraft selectedData;//선택한 데이터(사용하는 데이터와 같을 수도 있고 다를 수도 있음)
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
        aircraftDatas.Add(new Aircraft("C-15C", 0.03f, 330, 16, 150, 300, 10, 30, 500, 20, 10, 5, true, 0));//기체들 데이터 로딩 + 업그레이드데이터 로딩
        aircraftDatas.Add(new Aircraft("C-16A", 0.03f, 230, 12, 120, 300, 10, 25, 500, 15, 10, 5, true, 0));

        //PlayerPrefs.SetInt("MG-21S.Active", 0);
        //PlayerPrefs.SetInt("MG-29S.Active", 0);

        aircraftDatas.Add(new Aircraft("MG-21S", 0.029f, 370, 18, 80, 300, 10, 40, 400, 30, 10, 2, Convert.ToBoolean(PlayerPrefs.GetInt("MG-21S.Active", 0)), 20000));
        aircraftDatas.Add(new Aircraft("MG-29S", 0.035f, 280, 18, 150, 300, 10, 30, 500, 20, 10, 6, Convert.ToBoolean(PlayerPrefs.GetInt("MG-29S.Active", 0)), 30000));

        if (useMissileDatas == null)
        {
            useMissileDatas = new MissileData[] { WeaponDatas.instance.datas[0], WeaponDatas.instance.datas[0] };
        }

        UseStaticDataSet(PlayerPrefs.GetString("UseAircraftName", "C-16A"));
        //PlayerPrefs.SetString("UseAircraftName", useStaticData.name);

        //UpgradeResetDebug();//        
    }

    public void AircraftUnlockBtnDelete()
    {
        foreach(Aircraft data in aircraftDatas)
        {
            if (data.isActive)
            {
                HangerWdw.instance.AircraftUnlockBtnDelete(data.name);
            }
        }
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
        Aircraft data = FindAircraft(name);

        if (data.isActive)
        {
            useStaticData = data;//할당
            PlayerPrefs.SetString("UseAircraftName", name);//저장
        }

        MainSceneManager.Instance.HangerAircraftSet(name);
        selectedData = data;
    }
    public bool SelectedDataBuyTry()//구매버튼 누를시 호출
    {
        int cost = selectedData.cost;
        if(cost >= 20000)//전투데이터로 구매
        {
            if(GameManager.instance.combatData >= cost)
            {
                GameManager.instance.CombatDataUse(cost);
                SelectedDataUnlock();
                AircraftUnlockBtnDelete();
                return true;
            }
        }
        else if (cost < 20000)//골드로 구매
        {
            if (GameManager.instance.gold >= cost)
            {
                GameManager.instance.GoldUse(cost);
                SelectedDataUnlock();
                AircraftUnlockBtnDelete();
                return true;
            }
        }

        return false;
    }
    void SelectedDataUnlock()
    {
        selectedData.isActive = true;
        PlayerPrefs.SetInt(selectedData.name + ".Active", 1);
        useStaticData = selectedData;
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
        if (upgradeValue[(int)type] > 10)//업그레이드 최대치인경우
            return false;

        int upgradeCost = CostReturn(type);

        if (upgradeCost > 1000)//업그레이드에 소모하는 재화 종류가 전투데이터일경우
        {
            if(GameManager.instance.combatData >= upgradeCost)
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
            if(GameManager.instance.gold >= upgradeCost)
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

        if(upgradeValue[(int)type] >= 4 && upgradeValue[(int)type] <= 10)
        {
            return goldCost;
        }
        else if(upgradeValue[(int)type] < 4)
        {
            return dataCost;
        }
        else
        {
            return int.MaxValue;
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
