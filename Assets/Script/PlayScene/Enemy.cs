using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rigidbody;
    EnemyAiControl aiControl;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        aiControl = GetComponent<EnemyAiControl>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.velocity = transform.forward * 200;
        hp = max_Hp;
    }

    float airPressure;
    const float GLOBAL_DRAG = 0.03f;
    public float enginePower { get; set; }//0~1
    const float MAX_POWER = 10f;

    public float pitchAxis { get; set; }//-1~1
    const float MAX_PITCHMOMENT = 12;
    public float rollAxis { get; set; }//-1~1
    const float MAX_ROLLMOMENT = 20;
    public float yawAxis { get; set; }//-1~1
    const float MAX_YAWMOMENT = 10;

    const float MAX_LIFTPOWER = 40f;
    float hp;
    public float max_Hp;
    // Update is called once per frame
    void FixedUpdate()
    {
        airPressure = Mathf.Pow(1 - ((this.transform.position.y / 300) / 145.45f), 5.2561f);
        rigidbody.drag = GLOBAL_DRAG * airPressure * ((rigidbody.velocity.magnitude + 700) / 700);

        rigidbody.AddForce(this.transform.forward * enginePower * MAX_POWER * EnginePowerSet(), ForceMode.Force);
        if (MAX_LIFTPOWER < liftPower)//허용G에 따른 피칭모멘트 제한
        {
            pitchAxis *= (MAX_LIFTPOWER / liftPower);
        }
        if (pitchAxis > 0)//-G를 줄 시 피칭모멘트 감소
        {
            pitchAxis *= 0.3f;
        }
        float speedValue = Mathf.Clamp(((rigidbody.velocity.magnitude-40) * airPressure) * 0.004f, 0, 1);//속도에 반비례한 회전모멘트 감소
        rigidbody.AddTorque(this.transform.right * pitchAxis * speedValue * MAX_PITCHMOMENT, ForceMode.Force);
        rigidbody.AddTorque(this.transform.forward * rollAxis * speedValue * MAX_ROLLMOMENT, ForceMode.Force);
        rigidbody.AddTorque(this.transform.up * yawAxis * speedValue * MAX_YAWMOMENT, ForceMode.Force);


        AoaSet();
        LiftPowerSet();

        rigidbody.AddForce(this.transform.up * liftPower, ForceMode.Force);
        rigidbody.AddForce(this.transform.right * sideSlip * rigidbody.velocity.magnitude, ForceMode.Force);

        speed = rigidbody.velocity.magnitude;
    }
    public float aoa { get; private set; }
    float sideSlip;
    Vector3 aoaVec;
    const float STALL_AOA = 0.4f;
    public float speed { get; private set; }

    void AoaSet()//기체 자세에 따른 받음각 생성
    {
        aoaVec = transform.forward - rigidbody.velocity.normalized;//기체전방벡터 - 속도벡터 월드벡터 생성
        aoaVec = this.transform.InverseTransformDirection(aoaVec);//생성한 벡터를 현재 기체의 자세에 맞게 로컬벡터로 변환
        aoa = aoaVec.y;
        sideSlip = aoaVec.x;
    }
    float liftPower;

    void LiftPowerSet()//받음각과 스톨각에 따른 양력 생성
    {
        liftPower = rigidbody.velocity.magnitude * aoa * airPressure;
        if (aoa < STALL_AOA)
        {
            liftPower *= 2;
        }
    }

    float EnginePowerSet()
    {
        if (speed < 400)
        {
            return Mathf.Lerp(1, 2, speed * 0.0025f) * Mathf.Pow(airPressure, 0.5f);
        }
        else
        {
            return Mathf.Lerp(2, 1, speed * 0.0025f - 1) * Mathf.Pow(airPressure, 0.5f);
        }
    }

    public ParticleSystem gunHitEffect;
    public AudioSource gunHitSound;
    public void Hit(float dmg, bool gunEffectOn)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            GetComponent<Target>().Destroyed(this);
        }
        else if(gunEffectOn)
        {
            GunHitEffect();
        }
    }

    public Vector3 heatLv()
    {
        return this.transform.position - this.transform.forward * 3000;
    }

    void GunHitEffect()
    {
        gunHitEffect.Play();
        gunHitSound.Play();
    }

    //public int score = 100;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            GetComponent<Target>().Destroyed(this);
        }
    }
}
