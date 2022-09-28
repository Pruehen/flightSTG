using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSceneManager : MonoBehaviour
{
    public static MissionSceneManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isPause = false;
        PauseWndSet();
    }

    bool isPause = false;
    public GameObject pauseWdw;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PressESC();
        }        
    }
    
    public void PressESC()
    {
        isPause = !isPause;
        PauseWndSet();
    }

    void PauseWndSet()
    {
        if(!isPause)
        {
            Time.timeScale = 1.0f;
            pauseWdw.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseWdw.SetActive(true);   
        }
    }

    public void ToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ToDebriefingScene()
    {
        SceneManager.LoadScene("DebriefingScene");
    }
}
