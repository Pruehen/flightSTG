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
    }

    public GameObject ExitWdn;//'종료하시겠습니까?' 창
    public GameObject SettingWdw;//설정 창
    public GameObject MissionSelectWdw;//미션 선택 창

    public int selectedMissionNum = 0;

    private void Start()
    {
        selectedMissionNum = 0;
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

    public void GameExit()
    {
        Application.Quit();
    }

    public void ToPlayScene(int selectMission)
    {
        selectedMissionNum = selectMission;
        SceneManager.LoadScene("GamePlayScene");
    }
}
