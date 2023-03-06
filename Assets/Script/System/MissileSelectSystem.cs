using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissileSelectSystem : MonoBehaviour
{
    public Transform btnsTrsf;
    public List<MissileSelectBtn> missileSelectBtnList = new List<MissileSelectBtn>();
    public Image missileSpecValue1, missileSpecValue2, missileSpecValue3, missileSpecValue4;
    public TextMeshProUGUI seekerType;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < btnsTrsf.childCount; i++)
        {
            missileSelectBtnList.Add(btnsTrsf.GetChild(i).GetComponent<MissileSelectBtn>());
        }
        MissileSelectBtnSet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MissileSelectBtnSet()
    {
        foreach (MissileSelectBtn btn in missileSelectBtnList)
        {
            btn.BtnSet();
        }
    }


    public void MissileSpecGraphSet(MissileData missileData)
    {
        HangerWdw.instance.MissileSpecWdwToggle(true);

        missileSpecValue1.fillAmount = missileData.sensitivity / 50000;
        missileSpecValue2.fillAmount = missileData.MAX_BORESITE / 360;
        missileSpecValue3.fillAmount = (missileData.firstBurnTime + missileData.secondBurnTime*0.5f) * missileData.enginePower / 3000;
        missileSpecValue4.fillAmount = missileData.MAX_G / 600;

        switch(missileData.seekerType)
        {
            case 0:
                seekerType.text = "Heat tracking";
                break;
            case 1:
                seekerType.text = "Semi-active radar";
                break ;
            case 2:
                seekerType.text = "active radar";
                break;
        }
    }
}
