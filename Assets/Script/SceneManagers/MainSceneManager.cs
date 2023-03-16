using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public GameObject ExitWdn;//'�����Ͻðڽ��ϱ�?' â
    public GameObject SettingWdw;//���� â
    public GameObject MissionSelectWdw;//�̼� ���� â
    public GameObject HangerWdw;//�ݳ��� â    
    public GameObject Hanger;//�ݳ���

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
        Hanger.SetActive(value);
        HangerAircraftSet(AircraftManager.instance.useStaticData.name);
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

    public Transform aircrafts;
    public void HangerAircraftSet(string name)
    {
        if (Hanger.activeSelf == false)
            return;

        for (int i = 0; i < aircrafts.childCount; i++)
        {
            GameObject aircraft = aircrafts.GetChild(i).gameObject;
            if(aircraft.name == name)
            {
                aircraft.SetActive(true);
            }
            else
            {
                aircraft.SetActive(false);
            }
        }
    }

    public void AircraftSet(string name)
    {
        AircraftManager.instance.UseStaticDataSet(name);
    }
}
