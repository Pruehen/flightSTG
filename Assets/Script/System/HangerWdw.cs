using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HangerWdw : MonoBehaviour
{
    public TextMeshProUGUI combatDataText, goldText;

    public GameObject missileSelectWdw, missileSpecWdw, techTreeWdw, upgradeWdw;
    public Image selectedMissileImage1, selectedMissileImage2;
    public TextMeshProUGUI selectedMissileNameText1, selectedMissileNameText2;
    public GameObject buyBtn;
    public Transform aircraftSelectTrf;

    public static HangerWdw instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CombatDataTextSet();
        GoldTextSet();
        SelectMissileBtnSet();

        AircraftManager.instance.AircraftUnlockBtnDelete();
    }

    public void CombatDataTextSet()
    {
        combatDataText.text = GameManager.instance.combatData.ToString();
    }
    public void GoldTextSet()
    {
        goldText.text = GameManager.instance.gold.ToString();
    }

    public void MissileSelectWdwToggle(bool value)
    {
        missileSelectWdw.SetActive(value);
        missileSelectWdw.GetComponent<MissileSelectSystem>().MissileSelectBtnSet();
    }
    public void MissileSelectWdwIndexSet(int index)
    {
        AircraftManager.instance.IndexNumSet(index);  
    }

    public void MissileSpecWdwToggle(bool value)
    {
        missileSpecWdw.SetActive(value);
    }

    public void TechTreeWdwToggle(bool value)
    {
        techTreeWdw.SetActive(value);
    }

    public void UpgradeWdwToggle(bool value)
    {
        upgradeWdw.SetActive(value);
        if (AircraftUpgradeWdw.instance != null)
        {
            AircraftUpgradeWdw.instance.BtnSet();
        }
    }

    public void AircraftUnlockBtnDelete(string name)
    {
        if (aircraftSelectTrf.Find(name) != null)
        {
            aircraftSelectTrf.Find(name).gameObject.SetActive(false);
        }
    }

    [SerializeField]GameObject CurrentedAircraftUnlockBtn;
    public void AircraftUnlockBtnObjectCurrent(GameObject gameObject)
    {
        CurrentedAircraftUnlockBtn = gameObject;
    }

    public void BuyBtnToggle(bool value)
    {
        buyBtn.SetActive(value);
    }
    public void BuyBtnClick()
    {
        if(AircraftManager.instance.SelectedDataBuyTry())
        {
            buyBtn.SetActive(false);
        }
    }

    public void SelectMissileBtnSet()
    {
        selectedMissileNameText1.text = AircraftManager.instance.UseMissileData(0).missileName;
        selectedMissileNameText2.text = AircraftManager.instance.UseMissileData(1).missileName;
        SelectMissileBtnImageSet(AircraftManager.instance.UseMissileData(AircraftManager.instance.indexNum).missileName, AircraftManager.instance.indexNum);
    }

    public void SelectMissileBtnImageSet(string missileName, int index)
    {
        Image controlImage;
        if(index == 0)
        {
            controlImage = selectedMissileImage1;
        }
        else
        {
            controlImage = selectedMissileImage2;
        }

        if(missileName == "AIM-9J")
        {
            controlImage.sprite = MissileIconManager.instance.aim9j;
        }
        else if (missileName == "AIM-9G" || missileName == "AIM-9H" || missileName == "AIM-9C")
        {
            controlImage.sprite = MissileIconManager.instance.aim9g;
        }
        else if (missileName == "AIM-9M")
        {
            controlImage.sprite = MissileIconManager.instance.aim9m;
        }
        else if (missileName == "AIM-7E" || missileName == "AIM-7M")
        {
            controlImage.sprite = MissileIconManager.instance.aim7;
        }
        else if (missileName == "AIM-54A")
        {
            controlImage.sprite = MissileIconManager.instance.aim54;
        }
        else
        {
            controlImage.sprite = MissileIconManager.instance.aim9j;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
