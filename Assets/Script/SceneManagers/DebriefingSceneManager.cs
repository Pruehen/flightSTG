using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebriefingSceneManager : MonoBehaviour
{
    public void ToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
