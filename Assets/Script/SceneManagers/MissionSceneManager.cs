using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class MissionSceneManager : MonoBehaviour
{
    public static MissionSceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject pauseWdw;
    public TextMeshProUGUI scoreTxt;
    int score;


    private void Start()
    {
        pauseWdw.SetActive(false);
        if (MainManager.instance != null)
        {
            Debug.Log(MainSceneManager.Instance.selectedMissionNum);
        }
    }

    public void PauseWdwToggle(bool value)
    {
        pauseWdw.SetActive(value);
        if(value)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


    public void ToMainScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void ScoreUp(int upScore)
    {
        score += upScore;
        scoreTxt.text = "SCORE " + score;
    }
}
