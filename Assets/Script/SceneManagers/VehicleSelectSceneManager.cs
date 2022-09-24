using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleSelectSceneManager : MonoBehaviour
{
    public void ToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public GameObject InfoDataWdw;

    public void VehicleSelect(int num)
    {
        InfoDataWdw.SetActive(true);
        switch (num)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }


    public void ToMissionSelectScene()
    {
        SceneManager.LoadScene("MissionSelectScene");
    }
}
