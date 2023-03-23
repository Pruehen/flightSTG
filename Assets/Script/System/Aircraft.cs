using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft
{
    public string name;//�̸�
    public float dc = 0.03f;//�׷°��
    public float wL = 330;//�͸����� (t/m^2)
    public float MAX_POWER = 20f;//�ִ� �߷�
    public float MAX_PITCHMOMENT = 200;//��ġ���Ʈ
    public float MAX_ROLLMOMENT = 300;//�Ѹ��Ʈ
    public float MAX_YAWMOMENT = 10;//����Ʈ
    public float MAX_LIFTPOWER = 30f;//G���׼�
    public float max_hp = 500;//ü��
    public float STALL_AOA = 25;//�������׼�
    public float reloadTime = 10;//�������ð�
    public AircraftUpgradeData AircraftUpgradeData;//�ش��ü�� ���׷��̵� ������
    public bool isActive = false;//�ش��ü�� ��밡�ɿ���
    public int cost = 0;//��ü�� ����

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

