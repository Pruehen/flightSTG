using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;

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
    public TextMeshProUGUI combatDataText;//정산 완료 후 본인이 소유하고 있는 전투데이터의 양
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI combatDataText2;//이번판에 추가로 획득하는 전투데이터의 양

    public GoogleAds googlaAds;//보상 리워드에 사용되는 구글광고 관리클래스

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


        stageCombatData = (score + (stagePlayTime * 5) * 0.9f);

        if (isClear)
        {
            stageCombatData *= 1.5f;
            clearTxt.text = "CLEAR";
            clearTxt.color = Color.white;
            GameManager.instance.StageClear();
        }
        else
        {
            clearTxt.text = "FAILED";
            clearTxt.color = Color.gray;
        }
        GameManager.instance.CombatDataUse(-(int)stageCombatData);
        combatDataText2.text = ((int)stageCombatData).ToString();

        combatDataText.text = GameManager.instance.combatData.ToString();
        goldText.text = GameManager.instance.gold.ToString();
    }

    public void AdsView()
    {
        AudioListener.volume = 0;
        googlaAds.ShowAds();
    }

    public void PostADs()
    {
        AudioListener.volume = 1;
        GameManager.instance.CombatDataUse(-(int)stageCombatData);
        GameManager.instance.GoldUse(-1);
        ToMainScene();
    }
}
