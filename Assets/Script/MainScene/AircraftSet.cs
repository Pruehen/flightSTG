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
                seekerName = "������";
                break;
            case 1:
                seekerName = "�ݴɵ� ���̴� ����";
                break;
            case 2:
                seekerName = "�ɵ� ���̴� ����";
                break;
            default:
                seekerName = "����";
                break;
        }
        switch (missileData.bodyType)
        {
            case 0:
                bodyName = "�ʼ���";
                break;
            case 1:
                bodyName = "����";
                break;
            case 2:
                bodyName = "����";
                break;
            case 3:
                bodyName = "����";
                break;
            case 4:
                bodyName = "�ʴ���";
                break;
            default:
                bodyName = "���õ��� ����";
                break;
        }

        missileInfoText.text = "�̸� = " + missileName + "\n" +
            "�۵� �ð� = " + missileData.lifeTime.ToString() + "��" + "\n" +
            "���� �ð� 1 = " + missileData.firstBurnTime.ToString() + "��" + "\n" +
            "���� �ð� 2 = " + missileData.secondBurnTime.ToString() + "��" + "\n" +
            "�⵿�� = " + (missileData.MAX_G * 0.1f).ToString() + "G" + "\n" +
            "���ӷ� = " + missileData.enginePower.ToString() + "m/s^2" + "\n" +
            "������ = �ʴ� " + missileData.MAX_TURN_RATE.ToString() + "��" + "\n" +
            "�ִ� ������ = " + missileData.MAX_BORESITE.ToString() + "��" + "\n" +
            "�׷°�� = " + missileData.defaultDrag.ToString() + "\n" +
            "��Ŀ ���� = " + seekerName + "\n" +
            "��ü ũ�� = " + bodyName;
    }*/
}
