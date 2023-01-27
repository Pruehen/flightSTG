using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{

    public GameObject ExitWdn;//'종료하시겠습니까?' 창
    public GameObject SettingWdw;//설정 창
    public void ExitWdnToggle(bool value)
    {
        ExitWdn.SetActive(value);
    }

    public void SettingWdwToggle(bool value)
    {
        SettingWdw.SetActive(value);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void ToVehicleSelectScene()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
}
