using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft
{
    public string name;//이름
    public float dc = 0.03f;//항력계수
    public float wL = 330;//익면하중 (t/m^2)
    public float MAX_POWER = 20f;//최대 추력
    public float MAX_PITCHMOMENT = 200;//피치모멘트
    public float MAX_ROLLMOMENT = 300;//롤모멘트
    public float MAX_YAWMOMENT = 10;//요모멘트
    public float MAX_LIFTPOWER = 30f;//G저항성
    public float max_hp = 500;//체력
    public float STALL_AOA = 25;//스톨저항성
    public float reloadTime = 10;//재장전시간
    public AircraftUpgradeData AircraftUpgradeData;//해당기체의 업그레이드 데이터
    public bool isActive = false;//해당기체의 사용가능여부
    public int cost = 0;//기체의 가격

    public Aircraft(string name, float dc, float wL, float mAX_POWER, float mAX_PITCHMOMENT, float mAX_ROLLMOMENT, float mAX_YAWMOMENT, float mAX_LIFTPOWER, float max_hp, float sTALL_AOA, float reloadTime, int baseUpgradeCost, bool active, int cost)
    {
        this.name = name;
        this.dc = dc;
        this.wL = wL;
        MAX_POWER = mAX_POWER;
        MAX_PITCHMOMENT = mAX_PITCHMOMENT;
        MAX_ROLLMOMENT = mAX_ROLLMOMENT;
        MAX_YAWMOMENT = mAX_YAWMOMENT;
        MAX_LIFTPOWER = mAX_LIFTPOWER;
        this.max_hp = max_hp;
        STALL_AOA = sTALL_AOA;
        this.reloadTime = reloadTime;
        AircraftUpgradeData = new AircraftUpgradeData(name, baseUpgradeCost);
        this.isActive = active;
        this.cost = cost;
    }
}

