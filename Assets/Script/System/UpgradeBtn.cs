using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtn : MonoBehaviour
{
    public string upgradeName;

    public void BtnSet()
    {
        Image btnImage = this.transform.GetChild(1).GetComponent<Image>();
        TextMeshProUGUI btnCostTmp = this.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        Upgrade thisUpgrade = UpgradeManager.instance.Find(upgradeName);

        if (thisUpgrade.isUpgrade)
        {
            btnImage.color = Color.green;
        }
        else if(thisUpgrade.NeedNodeCheck(out string value))
        {            
            btnImage.color = Color.yellow;
        }
        else
        {
            btnImage.color = Color.black;
        }

        btnCostTmp.text = thisUpgrade.cost.ToString();
    }
}
