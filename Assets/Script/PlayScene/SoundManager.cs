using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]AudioSource missile_flyby,
        Gun_Hit; 

}
