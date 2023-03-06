using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Upgrade> UpgradeList = new List<Upgrade>();//런타임에서 사용되는 업그레이드리스트
    List<bool> UpgradeSaveData = new List<bool>();//런타임에서 사용되는 업그레이드여부 리스트. playerPrefs(string(upgradename), int (0 or 1)) 을 통해 저장됨
    public List<UpgradeBtn> upgradeBtns = new List<UpgradeBtn>();//버튼 리스트. 
    public Transform btnsTrsf;//에디터에서 할당해주는 버튼 트랜스폼

    bool DoUpgrade_Unlock(string upgradeName, int haveCombatData, out string result)
    {
        Upgrade upgrade = null;
        string insideResult = null;
        foreach (Upgrade item in UpgradeList)
        {
            if(item.upgradeName == upgradeName)
            {
                upgrade = item;
                break;
            }
        }
        if(upgrade == null)
        {
            result = "존재하지 않는 업그레이드입니다";
            return false;
        }

        if(upgrade.NeedNodeCheck(out insideResult) == false)
        {
            result = insideResult;
            return false;
        }

        if(upgrade.UpgradeTry_Unlock(haveCombatData, out insideResult) == false)
        {
            result = insideResult;
            return false;
        }
        else
        {
            result = insideResult;
            return true;
        }
    }
    public Upgrade Find(string upgradeName)
    {
        foreach(Upgrade upgrade in UpgradeList)
        {
            if(upgradeName == upgrade.upgradeName)
            {
                return upgrade;
            }
        }
        return null;
    }

    public void TryUpgrade_Unlock(string upgradeName)
    {
        if (DoUpgrade_Unlock(upgradeName, GameManager.instance.combatData, out string result) == true)
        {
            Debug.Log(result);
            UpgradeDataSave(upgradeName, true);
            UpgradeBtnsSet();
        }
        else
        {
            Debug.Log(result);
        }

    }

    public void TryUpgrade_Aircraft(int value)
    {
        UpgradeType upgradeType = (UpgradeType)value;

        AircraftManager.instance.useStaticData.AircraftUpgradeData.TryUpgrade(upgradeType);
        AircraftUpgradeWdw.instance.BtnSet();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpgradeList.Add(new Upgrade("Upgrade_AIM-9J", 1000));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-9G", 1000));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-9H", 3000, Find("Upgrade_AIM-9G")));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-9C", 3000, Find("Upgrade_AIM-9G")));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-9M", 10000, new Upgrade[] { Find("Upgrade_AIM-9J"), Find("Upgrade_AIM-9H") }));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-7E", 5000, Find("Upgrade_AIM-9C")));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-7M", 15000, Find("Upgrade_AIM-7E")));
        UpgradeList.Add(new Upgrade("Upgrade_AIM-54A", 20000, Find("Upgrade_AIM-7M")));

        UpgradeDataLoad();

        for (int i = 0; i < btnsTrsf.childCount; i++)
        {
            upgradeBtns.Add(btnsTrsf.GetChild(i).GetComponent<UpgradeBtn>());
        }

        UpgradeBtnsSet();

        //UpgradeReset_Debug();
    }

    void UpgradeDataLoad()
    {
        for (int i = 0; i < UpgradeList.Count; i++)
        {
            UpgradeSaveData.Add(false);
        }
        for (int i = 0; i < UpgradeList.Count; i++)
        {
            if (PlayerPrefs.GetInt(UpgradeList[i].upgradeName, 0) == 1)
            {
                UpgradeSaveData[i] = true;
                UpgradeList[i].DataLoad(true);
            }
        }
    }
    void UpgradeDataSave(string name, bool value)
    {
        for (int i = 0; i < UpgradeList.Count; i++)
        {
            if(name == UpgradeList[i].upgradeName)
            {
                if (value)
                {
                    PlayerPrefs.SetInt(name, 1);
                }
                else
                {
                    PlayerPrefs.SetInt(name, 0);
                }

            }
        }
    }

    public void UpgradeReset_Debug()
    {
        for (int i = 0; i < UpgradeList.Count; i++)
        {
            UpgradeDataSave(UpgradeList[i].upgradeName, false);
        }
        //GameManager.instance.CombatDataUp(1000);
    }

    void UpgradeBtnsSet()
    {
        foreach (UpgradeBtn upgradeBtn in upgradeBtns)
        {
            upgradeBtn.BtnSet();
        }
    }


}

public class Upgrade
{
    public string upgradeName { get; private set; }//업그레이드 이름
    public int cost { get; private set; }//업그레이드 가격
    public bool isUpgrade { get; private set; }//업그레이드 여부
    Upgrade[] needUpgradeNode;//업그레이드에 필요한 다른 업그레이드

    public Upgrade(string upgradeName, int cost, Upgrade[] needUpgradeNode)
    {
        this.upgradeName = upgradeName;
        this.cost = cost;
        this.isUpgrade = false;
        this.needUpgradeNode = needUpgradeNode;
    }
    public Upgrade(string upgradeName, int cost, Upgrade needUpgradeNode)
    {
        this.upgradeName = upgradeName;
        this.cost = cost;
        this.isUpgrade = false;
        this.needUpgradeNode = new Upgrade[] { needUpgradeNode };
    }
    public Upgrade(string upgradeName, int cost)
    {
        this.upgradeName = upgradeName;
        this.cost = cost;
        this.isUpgrade = false;
        this.needUpgradeNode = null;
    }

    public bool NeedNodeCheck(out string result)//업그레이드에 필요한 다른 업그레이드가 활성화되었는지. 되어있으면 true 반환
    {
        if(needUpgradeNode == null)
        {
            result = "업그레이드 가능";
            return true;
        }
        for(int i = 0; i < needUpgradeNode.Length; i++)
        {
            if(needUpgradeNode[i].isUpgrade == false)
            {
                result = "업그레이드에 필요한 다른 업그레이드를 활성화히십시오.";
                return false;
            }
        }
        result = "업그레이드 가능";
        return true;
    }

    public bool UpgradeTry_Unlock(int haveCombatData, out string result)
    {
        if(isUpgrade == true)
        {
            result = "이미 완료된 업그레이드입니다.";
            return false;
        }

        if(haveCombatData >= cost)
        {
            result = "업그레이드 성공";
            GameManager.instance.CombatDataUse(cost);            
            isUpgrade = true;
            return true;
        }
        else
        {
            result = "데이터가 모자랍니다.";
            return false;
        }
    }

    public void DataLoad(bool isUpgrade)
    {
        this.isUpgrade = isUpgrade;
    }
}
