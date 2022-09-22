using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class SystemMessage : MonoBehaviour
{
    public TextMeshProUGUI systemText;

    public void SystemTextSet(string text)
    {
        systemText.text = text;
    }
}
