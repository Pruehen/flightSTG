using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    }

    public void ToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ScoreUp(int upScore)
    {
        score += upScore;
        scoreTxt.text = "SCORE " + score;
    }
}
