using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public GameObject HPBAR;
    public float dc = 0.02f;//�׷°��
    public float wL;//�͸����� (t/m^2)
    float airPressure = 1;

    public float enginePower { get; set; }//0~1
    public float MAX_POWER = 16f;

    public float pitchAxis { get; set; }//-1~1
    public float MAX_PITCHMOMENT = 30;
    public float rollAxis { get; set; }//-1~1
    public float MAX_ROLLMOMENT = 80;
    public float yawAxis { get; set; }//-1~1
    public float MAX_YAWMOMENT = 5;

    public float MAX_LIFTPOWER = 15f;
    public float max_hp;
    float hp;

    public float reloadTime;

    public static PlayerInfo playerInfo;
    private void Awake()
    {
        playerInfo = this;
    }

    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        AircraftInit();

        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(0, 0, 200);
        hp = max_hp;

        Invoke("MissileImageSet", 0.1f);
    }

    void MissileImageSet()
    {
        for (int i = 0; i < 2; i++)
        {
            string misName = PlayerWeaponControl.instance.haveMissileDatas[i].missileName;
            if (misName == "AIM-9J")
            {
                missileImage[i].sprite = MissileIconManager.instance.aim9j;
            }
            else if (misName == "AIM-9G" || misName == "AIM-9H" || misName == "AIM-9C")
            {
                missileImage[i].sprite = MissileIconManager.instance.aim9g;
            }
            else if (misName == "AIM-9M")
            {
                missileImage[i].sprite = MissileIconManager.instance.aim9m;
            }
            else if (misName == "AIM-7M" || misName == "AIM-7E")
            {
                missileImage[i].sprite = MissileIconManager.instance.aim7;
            }
            else if (misName == "AIM-54A")
            {
                missileImage[i].sprite = MissileIconManager.instance.aim54;
            }
            missileImage[i + 2].sprite = missileImage[i].sprite;
        }
    }

    void AircraftInit()
    {
        Aircraft refData = AircraftManager.instance.useStaticData;

        dc = refData.dc;
        wL = refData.wL * (1/refData.AircraftUpgradeData.LightnessValue());
        MAX_POWER = refData.MAX_POWER * refData.AircraftUpgradeData.EngineValue();
        MAX_PITCHMOMENT = refData.MAX_PITCHMOMENT * (1 + refData.AircraftUpgradeData.LightnessValue() * 0.5f);
        MAX_ROLLMOMENT = refData.MAX_ROLLMOMENT * (1 + refData.AircraftUpgradeData.LightnessValue() * 0.5f);
        MAX_YAWMOMENT = refData.MAX_YAWMOMENT * (1 + refData.AircraftUpgradeData.LightnessValue() * 0.5f);
        MAX_LIFTPOWER = refData.MAX_LIFTPOWER * (1 + refData.AircraftUpgradeData.LightnessValue() * 0.5f);
        max_hp = refData.max_hp * (refData.AircraftUpgradeData.ArmorValue());
        STALL_AOA = refData.STALL_AOA;
        reloadTime = refData.reloadTime / (refData.AircraftUpgradeData.ReloadValue() * refData.AircraftUpgradeData.ReloadValue());
    }

    bool isStall = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        airPressure = Mathf.Pow(1 - ((this.transform.position.y / 300) / 145.45f), 5.2561f);
        rigidbody.drag = dc * airPressure * ((rigidbody.velocity.magnitude + 700) / 700);
        rigidbody.angularDrag = rigidbody.drag * 500;


        rigidbody.AddForce(this.transform.forward * enginePower * MAX_POWER * EnginePowerSet(), ForceMode.Force);
        if (MAX_LIFTPOWER < Mathf.Abs(liftPower))//���G�� ���� ��Ī���Ʈ ����
        {
            pitchAxis *= Mathf.Clamp((MAX_LIFTPOWER / Mathf.Abs(liftPower)), 0, 1);
        }
        //Debug.Log(pitchAxis);
        float speedValue = Mathf.Clamp(((rigidbody.velocity.magnitude - 30) * airPressure) * 0.001f, 0, 1);//�ӵ��� �ݺ���� ȸ�����Ʈ ����
        float aoaValue = 1;
        if (aoa > STALL_AOA)
        {
            aoaValue = Mathf.Clamp(STALL_AOA / aoa, 0, 1);
        }
        if (pitchAxis > 0)//��ġ �ٿ�
        {
            rigidbody.AddTorque(this.transform.right * pitchAxis * speedValue * aoaValue * 0.15f * MAX_PITCHMOMENT, ForceMode.Force);
        }
        else//��ġ ��
        {
            rigidbody.AddTorque(this.transform.right * pitchAxis * speedValue * aoaValue * MAX_PITCHMOMENT, ForceMode.Force);
        }

        rigidbody.AddTorque(this.transform.forward * -(rollAxis + yawAxis * 0.1f) * speedValue * MAX_ROLLMOMENT, ForceMode.Force);
        rigidbody.AddTorque(this.transform.up * yawAxis * speedValue * MAX_YAWMOMENT, ForceMode.Force);

        if (rigidbody.velocity.magnitude < 50 && !isStall)
        {
            isStall = true;
            rigidbody.angularDrag = 1;
        }
        else if (rigidbody.velocity.magnitude >= 50 && isStall)
        {
            isStall = false;
            rigidbody.angularDrag = 10;
        }

        AoaSet();
        LiftPowerSet();

        rigidbody.AddForce(this.transform.up * liftPower, ForceMode.Force);
        rigidbody.AddForce(this.transform.right * sideSlip * rigidbody.velocity.magnitude, ForceMode.Force);

        MapOutCheck();

        speed = rigidbody.velocity.magnitude;

    }

    [SerializeField] TextMeshProUGUI speedText, heightText, gunText, mslText, flrText, dmgText, aoaText, gforceText, maxGText;
    public GameObject velocityVector, aircraftCenter;
    public Image[] missileImage;

    public void GunTextSet(int ammu)
    {
        gunText.text = "GUN  " + ammu;
    }
    public void MslTextSet(string missile)
    {
        mslText.text = missile;
    }
    public void FlrTextSet(int ammu)
    {
        flrText.text = "FLR  " + ammu;
    }
    public void DmgTextSet(int hp)
    {
        dmgText.text = "DMG  " + ((int)(max_hp - hp) * 100 / (int)max_hp).ToString() + "%";
    }
    public void MissileImageSet(int missileIndex)
    {
        missileImage[missileIndex].fillAmount = -((PlayerWeaponControl.instance.missileCooldown[missileIndex] - reloadTime) / reloadTime);
    }
    public void MissileImageColorSet(int missileIndex)
    {
        if(missileIndex == 0)
        {
            missileImage[missileIndex].color = new Color(0, 255, 0);
            missileImage[missileIndex+2].color = new Color(0, 255, 0);
            missileImage[missileIndex+1].color = new Color(0, 255, 0, 0.2f);
            missileImage[missileIndex+3].color = new Color(0, 255, 0, 0.2f);
        }
        else if(missileIndex == 1)
        {
            missileImage[missileIndex].color = new Color(0, 255, 0);
            missileImage[missileIndex + 2].color = new Color(0, 255, 0);
            missileImage[missileIndex - 1].color = new Color(0, 255, 0, 0.2f);
            missileImage[missileIndex + 1].color = new Color(0, 255, 0, 0.2f);
        }
    }

    private void Update()
    {
        speedText.text = (int)(rigidbody.velocity.magnitude * 3.6f) + "km/h";
        heightText.text = (int)(this.transform.position.y) + "m";
        aoaText.text = "A " + string.Format("{0:N0}", aoa);
        gforceText.text = "G " + string.Format("{0:N0}", liftPower);
        maxGText.text = "90";

        Vector3 viewPos2 = Camera.main.WorldToViewportPoint(this.transform.position + rigidbody.velocity);
        viewPos2 = new Vector3(viewPos2.x - 0.5f, viewPos2.y - 0.5f, 0);
        velocityVector.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos2);

        Vector3 viewPos1 = Camera.main.WorldToViewportPoint(this.transform.position + this.transform.forward * 1000);
        viewPos1 = new Vector3(viewPos1.x - 0.5f, viewPos1.y - 0.5f, 0);
        aircraftCenter.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos1);
    }

    public float aoa { get; private set; }
    float sideSlip;
    Vector3 aoaVec;
    public float STALL_AOA;
    public float speed { get; private set; }

    void AoaSet()//��ü �ڼ��� ���� ������ ����
    {
        aoaVec = transform.forward - rigidbody.velocity.normalized;//��ü���溤�� - �ӵ����� ���庤�� ����
        aoaVec = this.transform.InverseTransformDirection(aoaVec);//������ ���͸� ���� ��ü�� �ڼ��� �°� ���ú��ͷ� ��ȯ
        aoa = Mathf.Asin(aoaVec.y) * Mathf.Rad2Deg;
        sideSlip = aoaVec.x;        
    }
    float liftPower;
    float cl;//��°�� 
    

    void LiftPowerSet()//�������� ���簢�� ���� ��� ����
    {
        if(aoa < -STALL_AOA)
        {
            cl = -3f;
        }
        else if(aoa >= -STALL_AOA && aoa < 0)
        {
            cl = Mathf.Lerp(0, -3f, -aoa / STALL_AOA);
        }
        else if(aoa >= 0 && aoa < STALL_AOA)
        {
            cl = Mathf.Lerp(0, 3f, aoa / STALL_AOA);
        }
        else if(aoa >= STALL_AOA)
        {
            cl = 3f;
        }
        liftPower = Mathf.Pow(rigidbody.velocity.magnitude, 2) * cl * airPressure * 0.5f / wL;

        //Debug.Log(liftPower);
    }

    void MapOutCheck()
    {
        if (this.transform.position.x > 30000 || this.transform.position.x < -30000 || this.transform.position.z > 30000 || this.transform.position.z < -30000)
        {
            this.transform.position = this.transform.position * 0.99f;
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, rigidbody.velocity.y, -rigidbody.velocity.z) * 0.5f;
            this.transform.LookAt(rigidbody.velocity);
        }        
    }

    float EnginePowerSet()
    {
        if(speed < 400)
        {
            return Mathf.Lerp(1, 1.2f, speed * 0.0025f) * Mathf.Pow(airPressure, 0.5f);
        }
        else
        {
            return Mathf.Lerp(1.2f, 1, speed * 0.0025f - 1) * Mathf.Pow(airPressure, 0.5f);
        }
    }   

    public void Hit(float dmg)
    {
        hp -= dmg;
        HpBarColorSet();
        DmgTextSet((int)hp);

        if (hp <= 0)
        {
            MissionSceneManager.instance.GameEnd(false);
        }
    }

    public void HpBarColorSet()
    {
        switch (hp / max_hp * (max_hp / 100))
        {
            case 0:
                HPBAR.GetComponent<Image>().color = new Color(0f, 0f, 0f);
                break;
            case 1:
                HPBAR.GetComponent<Image>().color = new Color(1f, 0f, 0f);
                break;
            case 2:
                HPBAR.GetComponent<Image>().color = new Color(1f, 0.5f, 0f);
                break;
            case 3:
                HPBAR.GetComponent<Image>().color = new Color(1f, 1f, 0f);
                break;
            case 4:
                HPBAR.GetComponent<Image>().color = new Color(0.5f, 1f, 0f);
                break;
            case 5:
                HPBAR.GetComponent<Image>().color = new Color(0, 1, 0);
                break;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            MissionSceneManager.instance.GameEnd(false);
        }
    }
}
