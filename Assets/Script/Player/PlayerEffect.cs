using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public float enginePower;

    public ParticleSystem rEngineParticle;
    public ParticleSystem lEngineParticle;

    public ParticleSystem[] voltexParticles = new ParticleSystem[4];

    public ParticleSystem sonicBoomParticle;
    // Start is called before the first frame update
    void Start()
    {
        rEnginePosition = rEngineParticle.transform.localPosition;
        lEnginePosition = lEngineParticle.transform.localPosition;
    }

    bool isPitching = false;
    bool isHighAlpha = false;

    Vector3 rEnginePosition;
    Vector3 lEnginePosition;

    // Update is called once per frame
    void Update()
    {
        enginePower = PlayerInfo.playerInfo.enginePower;

        rEngineParticle.gameObject.transform.localPosition = new Vector3(rEnginePosition.x, rEnginePosition.y, 10 - enginePower * 10);
        lEngineParticle.gameObject.transform.localPosition = new Vector3(lEnginePosition.x, lEnginePosition.y, 10 - enginePower * 10);

        if (PlayerInfo.playerInfo.aoa > PlayerInfo.playerInfo.STALL_AOA * 0.9f && !isHighAlpha)
        {
            isHighAlpha = true;
            voltexParticles[0].Play();
            voltexParticles[1].Play();
        }
        else if (PlayerInfo.playerInfo.aoa <= PlayerInfo.playerInfo.STALL_AOA * 0.9f && isHighAlpha)
        {
            isHighAlpha = false;
            voltexParticles[0].Stop();
            voltexParticles[1].Stop();
        }

        if (PlayerInfo.playerInfo.aoa > PlayerInfo.playerInfo.STALL_AOA * 0.2f && !isPitching)
        {
            isPitching = true;
            voltexParticles[2].Play();
            voltexParticles[3].Play();
        }
        else if (PlayerInfo.playerInfo.aoa <= PlayerInfo.playerInfo.STALL_AOA * 0.2f && isPitching)
        {
            isPitching = false;
            voltexParticles[2].Stop();
            voltexParticles[3].Stop();
        }

        if ((int)PlayerInfo.playerInfo.speed == 330)
        {
            sonicBoomParticle.Play();
        }
    }
}
