using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AircraftUpgradeWdw : MonoBehaviour
{
    public static AircraftUpgradeWdw instance;

    private void Awake()
    {
        instance = this;
    }

    public List<TextMeshProUGUI> upgradeCostTmps = new List<TextMeshProUGUI>();
    public List<Image> aircraftStockSpecImage = new List<Image>();
    public List<Image> aircraftUpgradeSpecImage = new List<Image>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnSet()
    {
        List<int> costTemp = new List<int>();

        costTemp.Add(AircraftManager.instance.useStaticData.AircraftUpgradeData.CostReturn(UpgradeType.engine));
        costTemp.Add(AircraftManager.instance.useStaticData.AircraftUpgradeData.CostReturn(UpgradeType.lightness));
        costTemp.Add(AircraftManager.instance.useStaticData.AircraftUpgradeData.CostReturn(UpgradeType.armor));
        costTemp.Add(AircraftManager.instance.useStaticData.AircraftUpgradeData.CostReturn(UpgradeType.reload));

        for (int i = 0; i < costTemp.Count; i++)
        {
            if (costTemp[i] == int.MaxValue)
                upgradeCostTmps[i].text = "MAX UPGRADE";
            else
                upgradeCostTmps[i].text = costTemp[i].ToString();
        }

        aircraftStockSpecImage[0].fillAmount = AircraftManager.instance.useStaticData.MAX_POWER * 0.02f;//엔진 (최대출력)
        aircraftStockSpecImage[1].fillAmount = 100 / AircraftManager.instance.useStaticData.wL;//무게 (익면하중)
        aircraftStockSpecImage[2].fillAmount = AircraftManager.instance.useStaticData.max_hp * 0.0008f;//장갑 (체력)
        aircraftStockSpecImage[3].fillAmount = AircraftManager.instance.useStaticData.MAX_POWER * 0.02f;//재장전 (재장전계수)


        aircraftUpgradeSpecImage[0].fillAmount = aircraftStockSpecImage[0].fillAmount * AircraftManager.instance.useStaticData.AircraftUpgradeData.engineValue;
        aircraftUpgradeSpecImage[1].fillAmount = aircraftStockSpecImage[1].fillAmount * AircraftManager.instance.useStaticData.AircraftUpgradeData.lightnessValue;
        aircraftUpgradeSpecImage[2].fillAmount = aircraftStockSpecImage[2].fillAmount * AircraftManager.instance.useStaticData.AircraftUpgradeData.armorValue;
        aircraftUpgradeSpecImage[3].fillAmount = aircraftStockSpecImage[3].fillAmount * AircraftManager.instance.useStaticData.AircraftUpgradeData.reloadValue;

    }
}
