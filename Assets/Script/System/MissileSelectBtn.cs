using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileSelectBtn : MonoBehaviour
{
    public string upgradeName;
    string missileName;
    bool isActive = false;
    MissileData haveMissileData;
    public MissileSelectSystem missileSelectSystem;


    public void BtnSet()
    {
        Image btnImage = this.transform.GetChild(1).GetComponent<Image>();

        Upgrade thisUpgrade = UpgradeManager.instance.Find(upgradeName);

        if (thisUpgrade.isUpgrade)
        {
            btnImage.color = Color.green;
            isActive = true;
        }
        else
        {
            btnImage.color = Color.black;
            isActive = false;
        }

        missileName = upgradeName.Split('_')[1];

        haveMissileData = WeaponDatas.instance.Find(missileName);
    }

    public void MissileSelect()
    {
        AircraftManager.instance.MissileSet(haveMissileData);
        missileSelectSystem.MissileSpecGraphSet(haveMissileData);
        HangerWdw.instance.SelectMissileBtnSet();
    }
}
