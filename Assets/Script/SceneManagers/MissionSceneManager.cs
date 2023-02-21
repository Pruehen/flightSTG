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
        PauseSet(true);
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
        Time.timeScale = 1;
        Invoke("SoundOn", 0.5f);
    }

    void SoundOn()
    {
        AudioListener.volume = 1;
    }

    public void PauseWdwToggle(bool value)
    {
        pauseWdw.SetActive(value);
        PauseSet(value);
    }


    public void ToMainScene()
    {
        PauseSet(false);
        SceneManager.LoadScene("MainScene");
    }

    public void ScoreUp(int upScore)
    {
        score += upScore;
        scoreTxt.text = "SCORE " + score;
    }

    public void PauseSet(bool isPause)
    {
        if(isPause)
        {
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        else if(!isPause)
        {
            Time.timeScale = 1;
            AudioListener.volume = 1;
        }
    }    
}
