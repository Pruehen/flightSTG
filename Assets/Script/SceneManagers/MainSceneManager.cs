using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{

    public GameObject ExitWdn;//'종료하시겠습니까?' 창
    public void ExitWdnToggle(bool value)
    {
        ExitWdn.SetActive(value);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void ToVehicleSelectScene()
    {
        SceneManager.LoadScene("VehicleSelectScene");
    }
}
