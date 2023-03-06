using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageBtnSet : MonoBehaviour
{
    public void StageBtnInit()
    {
        int stageNum = 1;
        Debug.Log(GameManager.instance.clearStageNum);

        for (int i = 0; i < this.transform.childCount; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject btn = this.transform.GetChild(i).GetChild(j).gameObject;
                btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = stageNum.ToString();
                if (GameManager.instance.clearStageNum + 1 >= stageNum)
                {
                    btn.GetComponent<Image>().color = Color.white;
                    btn.GetComponent<Button>().onClick.AddListener(MainSceneManager.Instance.ToPlayScene);
                }
                else
                {
                    btn.GetComponent<Image>().color = Color.gray;
                }

                stageNum++;
            }
        }
    }
    public void StageDiffSet(int value)
    {
        GameManager.instance.DifficultySet(value);
    }
}
