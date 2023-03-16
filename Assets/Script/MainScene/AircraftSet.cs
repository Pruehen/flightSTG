using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AircraftSet : MonoBehaviour
{
    /*public static AircraftSet instance;

    public TextMeshProUGUI missileInfoText;
    private void Awake()
    {
        instance = this;
    }

    public void MissileSelectBtnClick(int index)
    {
        SaveData infoData = MissileSaveLoadManager.instance.GetSaveData(index);

        string missileName = infoData.name;
        MissileData missileData = MissileSaveLoadManager.instance.FloatToData(infoData.data);

        string seekerName;
        string bodyName;
        switch (missileData.seekerType)
        {
            case 0:
                seekerName = "열추적";
                break;
            case 1:
                seekerName = "반능동 레이더 추적";
                break;
            case 2:
                seekerName = "능동 레이더 추적";
                break;
            default:
                seekerName = "없음";
                break;
        }
        switch (missileData.bodyType)
        {
            case 0:
                bodyName = "초소형";
                break;
            case 1:
                bodyName = "소형";
                break;
            case 2:
                bodyName = "중형";
                break;
            case 3:
                bodyName = "대형";
                break;
            case 4:
                bodyName = "초대형";
                break;
            default:
                bodyName = "선택되지 않음";
                break;
        }

        missileInfoText.text = "이름 = " + missileName + "\n" +
            "작동 시간 = " + missileData.lifeTime.ToString() + "초" + "\n" +
            "연소 시간 1 = " + missileData.firstBurnTime.ToString() + "초" + "\n" +
            "연소 시간 2 = " + missileData.secondBurnTime.ToString() + "초" + "\n" +
            "기동성 = " + (missileData.MAX_G * 0.1f).ToString() + "G" + "\n" +
            "가속력 = " + missileData.enginePower.ToString() + "m/s^2" + "\n" +
            "추적률 = 초당 " + missileData.MAX_TURN_RATE.ToString() + "도" + "\n" +
            "최대 추적각 = " + missileData.MAX_BORESITE.ToString() + "도" + "\n" +
            "항력계수 = " + missileData.defaultDrag.ToString() + "\n" +
            "시커 종류 = " + seekerName + "\n" +
            "동체 크기 = " + bodyName;
    }*/
}
