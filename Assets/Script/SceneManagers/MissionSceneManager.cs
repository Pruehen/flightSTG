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

    public GameObject gameEndWdw;
    public TextMeshProUGUI clearTxt;
    public TextMeshProUGUI combatDataText;//���� �Ϸ� �� ������ �����ϰ� �ִ� ������������ ��
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI combatDataText2;//�̹��ǿ� �߰��� ȹ���ϴ� ������������ ��


    private void Start()
    {
        pauseWdw.SetActive(false);

        AudioListener.volume = 0;
        Time.timeScale = 1;
        Invoke("SoundOn", 0.5f);
    }

    float stagePlayTime = 0;
    private void Update()
    {
        stagePlayTime += Time.deltaTime;
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
        }
        else if(!isPause)
        {
            Time.timeScale = 1;
        }
    }

    float stageCombatData = 0;
    
    public void GameEnd(bool isClear)
    {
        PauseSet(true);
        gameEndWdw.SetActive(true);

        stageCombatData = score + (stagePlayTime * 5);

        if (isClear)
        {
            clearTxt.text = "CLEAR";
            clearTxt.color = Color.white;
            GameManager.instance.CombatDataUse(-(int)(stageCombatData * 1.5f));
            combatDataText2.text = ((int)(stageCombatData * 1.5f)).ToString();
            GameManager.instance.StageClear();
        }
        else
        {
            clearTxt.text = "FAILED";
            clearTxt.color = Color.gray;
            GameManager.instance.CombatDataUse(-(int)stageCombatData);
            combatDataText2.text = ((int)stageCombatData).ToString();
        }

        combatDataText.text = GameManager.instance.combatData.ToString();
        goldText.text = GameManager.instance.gold.ToString();
    }
}
