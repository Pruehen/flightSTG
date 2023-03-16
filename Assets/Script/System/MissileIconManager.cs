using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileIconManager : MonoBehaviour
{
    public static MissileIconManager instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Sprite aim9g, aim9j, aim9m, aim7, aim54;
}
