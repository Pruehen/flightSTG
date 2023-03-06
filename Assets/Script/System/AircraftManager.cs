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
    public string useAircraftName = "C-15C";
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
        aircraftDatas.Add(new Aircraft("C-15C", 0.03f, 330, 20, 150, 300, 10, 30, 500, 20, 10, 5000));//기체들 데이터 로딩 + 업그레이드데이터 로딩

        if (useMissileDatas == null)
        {
            useMissileDatas = new MissileData[] { WeaponDatas.instance.datas[0], WeaponDatas.instance.datas[0] };
        }

        useStaticData = FindAircraft(useAircraftName);

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
    public float engineValue { get; private set; }
    public float lightnessValue { get; private set; }
    public float armorValue { get; private set; }
    public float reloadValue { get; private set; }
    public int baseCost { get; private set; }

    public AircraftUpgradeData (string name, int cost)
    {
        this.name = name;
        this.engineValue = PlayerPrefs.GetFloat(name + "_engineValue", 1);
        this.lightnessValue = PlayerPrefs.GetFloat(name + "_lightnessValue", 1);
        this.armorValue = PlayerPrefs.GetFloat(name + "_armorValue", 1);
        this.reloadValue = PlayerPrefs.GetFloat(name + "_reloadValue", 1);
        baseCost = cost;
    }

    public bool TryUpgrade(UpgradeType type)
    {        
        switch (type)
        {
            case UpgradeType.engine:
                if (engineValue >= 1.5f)
                    return false;
                else
                {
                    if(GameManager.instance.CombatDataUse((int)CostReturn(type)))
                    {
                        engineValue += 0.1f;
                        PlayerPrefs.SetFloat(name + "_engineValue", engineValue);
                        return true;
                    }
                }
                break;
            case UpgradeType.lightness:
                if (lightnessValue >= 1.5f)
                    return false;
                else
                {
                    if (GameManager.instance.CombatDataUse((int)CostReturn(type)))
                    {
                        lightnessValue += 0.1f;
                        PlayerPrefs.SetFloat(name + "_lightnessValue", lightnessValue);
                        return true;
                    }
                }
                break;
            case UpgradeType.armor:
                if (armorValue >= 1.5f)
                    return false;
                else
                {
                    if (GameManager.instance.CombatDataUse((int)CostReturn(type)))
                    {
                        armorValue += 0.1f;
                        PlayerPrefs.SetFloat(name + "_armorValue", armorValue);
                        return true;
                    }
                }
                break;
            case UpgradeType.reload:
                if (reloadValue >= 1.5f)
                    return false;
                else
                {
                    if (GameManager.instance.CombatDataUse((int)CostReturn(type)))
                    {
                        reloadValue += 0.1f;
                        PlayerPrefs.SetFloat(name + "_reloadValue", reloadValue);
                        return true;
                    }
                }
                break;
            default:
                return false;
        }

        return false;
    }

    public int CostReturn(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.engine:
                if (engineValue >= 1.5f)
                    return int.MaxValue;
                else
                {
                    return baseCost * (int)Mathf.Pow(engineValue, 5);
                }
            case UpgradeType.lightness:
                if (lightnessValue >= 1.5f)
                    return int.MaxValue;
                else
                {
                    return baseCost * (int)Mathf.Pow(lightnessValue, 5);
                }
            case UpgradeType.armor:
                if (armorValue >= 1.5f)
                    return int.MaxValue;
                else
                {
                    return baseCost * (int)Mathf.Pow(armorValue, 5);
                }
            case UpgradeType.reload:
                if (reloadValue >= 1.5f)
                    return int.MaxValue;
                else
                {
                    return baseCost * (int)Mathf.Pow(reloadValue, 5);
                }
            default:
                return 0;
        }
    }

    public void UpgradeReset()
    {
        engineValue = 1;
        lightnessValue = 1;
        armorValue = 1;
        reloadValue = 1;
        PlayerPrefs.SetFloat(name + "_engineValue", 1);
        PlayerPrefs.SetFloat(name + "_lightnessValue", 1);
        PlayerPrefs.SetFloat(name + "_armorValue", 1);
        PlayerPrefs.SetFloat(name + "_reloadValue", 1);
    }
}
