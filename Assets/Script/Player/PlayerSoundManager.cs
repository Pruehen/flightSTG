using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource engineSound_01;
    public AudioSource engineSound_02;
    public AudioSource gunSound;
    public AudioSource missileSeekerSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            engineSound_01.pitch = PlayerInfo.playerInfo.enginePower * 0.5f + 1;
            engineSound_02.volume = PlayerInfo.playerInfo.enginePower * 0.25f;
        }        
    }    
}
