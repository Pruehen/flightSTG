using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        PlayerPrefs.Save();
    }

    public GameObject ExitWdn;//'종료하시겠습니까?' 창
    public GameObject SettingWdw;//설정 창
    public GameObject MissionSelectWdw;//미션 선택 창
    public GameObject HangerWdw;//격납고 창    

    private void Start()
    {

    }

    public void ExitWdnToggle(bool value)
    {
        ExitWdn.SetActive(value);
    }

    public void SettingWdwToggle(bool value)
    {
        SettingWdw.SetActive(value);
    }

    public void MissionSelectWdwToggle(bool value)
    {
        MissionSelectWdw.SetActive(value);
    }

    public void HangerWdwToggle(bool value)
    {
        HangerWdw.SetActive(value);
    }

    public void GameExit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void ToPlayScene()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
}
