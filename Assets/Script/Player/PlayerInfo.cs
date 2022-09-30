using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public GameObject HPBAR;
    public float dc = 0.03f;//항력계수
    public float wL;//익면하중 (t/m^2)
    float airPressure = 1;

    public float enginePower { get; set; }//0~1
    public float MAX_POWER = 16f;

    public float pitchAxis { get; set; }//-1~1
    public float MAX_PITCHMOMENT = 30;
    public float rollAxis { get; set; }//-1~1
    public float MAX_ROLLMOMENT = 80;
    public float yawAxis { get; set; }//-1~1
    public float MAX_YAWMOMENT = 5;

    public float MAX_LIFTPOWER = 40f;
    public float max_hp;
    float hp;

    public static PlayerInfo playerInfo;
    private void Awake()
    {
        playerInfo = this;
    }

    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(0, 0, 200);
        hp = max_hp;
    }

    bool isStall = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        airPressure = Mathf.Pow(1 - ((this.transform.position.y / 300) / 145.45f), 5.2561f);
        rigidbody.drag = dc * airPressure * ((rigidbody.velocity.magnitude + 700) / 700);
        rigidbody.angularDrag = rigidbody.drag * 500;


        rigidbody.AddForce(this.transform.forward * enginePower * MAX_POWER * EnginePowerSet(), ForceMode.Force);
        if (MAX_LIFTPOWER < Mathf.Abs(liftPower))//허용G에 따른 피칭모멘트 제한
        {
            pitchAxis *= Mathf.Clamp((MAX_LIFTPOWER / Mathf.Abs(liftPower)), 0, 1);
        }
        //Debug.Log(pitchAxis);
        float speedValue = Mathf.Clamp(((rigidbody.velocity.magnitude - 30) * airPressure) * 0.003f, 0, 1);//속도에 반비례한 회전모멘트 감소
        float aoaValue = 1;
        if (aoa > STALL_AOA)
        {
            aoaValue = Mathf.Clamp(STALL_AOA / aoa, 0, 1);
        }
        if(pitchAxis > 0)//피치 다운
        {
            rigidbody.AddTorque(this.transform.right * pitchAxis * speedValue * aoaValue * 0.15f * MAX_PITCHMOMENT, ForceMode.Force);
        }
        else//피치 업
        {
            rigidbody.AddTorque(this.transform.right * pitchAxis * speedValue * aoaValue * MAX_PITCHMOMENT, ForceMode.Force);
        }

        rigidbody.AddTorque(this.transform.forward * -(rollAxis + yawAxis * 0.1f) * speedValue * MAX_ROLLMOMENT, ForceMode.Force);
        rigidbody.AddTorque(this.transform.up * yawAxis * speedValue * MAX_YAWMOMENT, ForceMode.Force);

        if(rigidbody.velocity.magnitude < 50 && !isStall)
        {
            isStall = true;
            rigidbody.angularDrag = 1;
        }
        else if(rigidbody.velocity.magnitude >= 50 && isStall)
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

    [SerializeField] TextMeshProUGUI speedText, heightText, HPPOINT;

    private void Update()
    {
        speedText.text = (int)(rigidbody.velocity.magnitude * 3.6f) + "km/h";
        heightText.text = (int)(this.transform.position.y) + "m";
        HpBarColor();
        HPPOINT.text = "HP " + (int)(hp / 5) + " %";
    }

    public float aoa { get; private set; }
    float sideSlip;
    Vector3 aoaVec;
    public float STALL_AOA;
    public float speed { get; private set; }

    void AoaSet()//기체 자세에 따른 받음각 생성
    {
        aoaVec = transform.forward - rigidbody.velocity.normalized;//기체전방벡터 - 속도벡터 월드벡터 생성
        aoaVec = this.transform.InverseTransformDirection(aoaVec);//생성한 벡터를 현재 기체의 자세에 맞게 로컬벡터로 변환
        aoa = Mathf.Asin(aoaVec.y) * Mathf.Rad2Deg;
        sideSlip = aoaVec.x;        
    }
    float liftPower;
    float cl;//양력계수 
    

    void LiftPowerSet()//받음각과 스톨각에 따른 양력 생성
    {
        if(aoa < -STALL_AOA)
        {
            cl = -0.5f;
        }
        else if(aoa >= -STALL_AOA && aoa < 0)
        {
            cl = Mathf.Lerp(0, -0.5f, -aoa / STALL_AOA);
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
            return Mathf.Lerp(1, 1.5f, speed * 0.0025f) * Mathf.Pow(airPressure, 0.5f);
        }
        else
        {
            return Mathf.Lerp(1.5f, 1, speed * 0.0025f - 1) * Mathf.Pow(airPressure, 0.5f);
        }
    }   

    public void Hit(float dmg)
    {
        hp -= dmg;
        Debug.Log(hp);

        if(hp <= 0)
        {
            MissionSceneManager.instance.ToDebriefingScene();
        }
    }

    public void HpBarColor()
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
            MissionSceneManager.instance.ToDebriefingScene();
        }
    }
}
