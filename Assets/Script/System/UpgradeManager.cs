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

    public List<Upgrade> UpgradeList = new List<Upgrade>();//��Ÿ�ӿ��� ���Ǵ� ���׷��̵帮��Ʈ
    List<bool> UpgradeSaveData = new List<bool>();//��Ÿ�ӿ��� ���Ǵ� ���׷��̵忩�� ����Ʈ. playerPrefs(string(upgradename), int (0 or 1)) �� ���� �����
    public List<UpgradeBtn> upgradeBtns = new List<UpgradeBtn>();//��ư ����Ʈ. 
    public Transform btnsTrsf;//�����Ϳ��� �Ҵ����ִ� ��ư Ʈ������

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
            result = "�������� �ʴ� ���׷��̵��Դϴ�";
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
        if(AircraftManager.instance.selectedData.isActive == false)
        {
            return;
        }
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
        //------------------------
        UpgradeList.Add(new Upgrade("Upgrade_R13M1", 3000, Find("Upgrade_AIM-9J")));
        UpgradeList.Add(new Upgrade("Upgrade_R23T", 10000, Find("Upgrade_R13M1")));
        UpgradeList.Add(new Upgrade("Upgrade_R23R", 5000, Find("Upgrade_R23T")));
        UpgradeList.Add(new Upgrade("Upgrade_R24T", 10000, Find("Upgrade_R23T")));
        UpgradeList.Add(new Upgrade("Upgrade_R24R", 5000, Find("Upgrade_R24T")));
        UpgradeList.Add(new Upgrade("Upgrade_R27T", 20000, Find("Upgrade_R24T")));
        UpgradeList.Add(new Upgrade("Upgrade_R27R", 10000, Find("Upgrade_R27T")));
        UpgradeList.Add(new Upgrade("Upgrade_R60", 5000, Find("Upgrade_R13M1")));
        UpgradeList.Add(new Upgrade("Upgrade_R73", 40000, Find("Upgrade_R60")));


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

        UpgradeSaveData[0] = true;//aim9j �⺻�����Ϸ�
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
    public string upgradeName { get; private set; }//���׷��̵� �̸�
    public int cost { get; private set; }//���׷��̵� ����
    public bool isUpgrade { get; private set; }//���׷��̵� ����
    Upgrade[] needUpgradeNode;//���׷��̵忡 �ʿ��� �ٸ� ���׷��̵�

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

    public bool NeedNodeCheck(out string result)//���׷��̵忡 �ʿ��� �ٸ� ���׷��̵尡 Ȱ��ȭ�Ǿ�����. �Ǿ������� true ��ȯ
    {
        if(needUpgradeNode == null)
        {
            result = "���׷��̵� ����";
            return true;
        }
        for(int i = 0; i < needUpgradeNode.Length; i++)
        {
            if(needUpgradeNode[i].isUpgrade == false)
            {
                result = "���׷��̵忡 �ʿ��� �ٸ� ���׷��̵带 Ȱ��ȭ���ʽÿ�.";
                return false;
            }
        }
        result = "���׷��̵� ����";
        return true;
    }

    public bool UpgradeTry_Unlock(int haveCombatData, out string result)
    {
        if(isUpgrade == true)
        {
            result = "�̹� �Ϸ�� ���׷��̵��Դϴ�.";
            return false;
        }

        if(haveCombatData >= cost)
        {
            result = "���׷��̵� ����";
            GameManager.instance.CombatDataUse(cost);            
            isUpgrade = true;
            return true;
        }
        else
        {
            result = "�����Ͱ� ���ڶ��ϴ�.";
            return false;
        }
    }

    public void DataLoad(bool isUpgrade)
    {
        this.isUpgrade = isUpgrade;
    }
}
