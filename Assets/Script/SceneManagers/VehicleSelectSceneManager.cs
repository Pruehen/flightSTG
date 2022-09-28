using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VehicleSelectSceneManager : MonoBehaviour
{
    
    public void ToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public GameObject InfoDataWdw;
    public TextMeshProUGUI infoTmp;

    public PlaneName selectedPlane;
    public void VehicleSelect(int num)
    {
        InfoDataWdw.SetActive(true);
        switch (num)
        {
            case 0:
                selectedPlane = PlaneName.F_18;
                break;
            case 1:
                selectedPlane = PlaneName.F_15;
                break;
            case 2:
                selectedPlane = PlaneName.AF_07;
                break;
            default:
                break;
        }

        infoTmp.text = selectedPlane.ToString();
    }


    public void ToMissionSelectScene()
    {
        SceneManager.LoadScene("MissionSelectScene");
    }
}

public enum PlaneName
{
    F_18,
    F_15,
    AF_07
}
