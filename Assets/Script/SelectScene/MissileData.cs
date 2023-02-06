using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissileData
{
    public float lifeTime = -1;
    public float firstBurnTime = -1;
    public float secondBurnTime = -1;
    public float MAX_G = -1;
    public float enginePower = -1;
    public float MAX_TURN_RATE = -1;
    public float MAX_BORESITE = -1;
    public float defaultDrag = -1;
    public int seekerType = -1;//열추적 0, 반능동 1, 능동 2;
    public float sensitivity = -1;
    public string missileName = null;

    public MissileData(float lifeTime, float fb, float sb, float maxG, float enginePower, float maxTurnRate, float boreSite, float drag, int seekerType, float sensitivity, string missileName)
    {
        this.lifeTime = lifeTime;
        this.firstBurnTime = fb;
        this.secondBurnTime = sb;
        this.MAX_G = maxG;
        this.enginePower = enginePower;
        this.MAX_TURN_RATE = maxTurnRate;
        this.MAX_BORESITE = boreSite;
        this.defaultDrag = drag;
        this.seekerType = seekerType;
        this.sensitivity = sensitivity;
        this.missileName = missileName;
    }
}
