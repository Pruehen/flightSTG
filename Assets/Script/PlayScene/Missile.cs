using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Missile : MonoBehaviour
{
    float lifeTime;
    float burnTime;
    float secondBurnTime;
    float MAX_G;
    float enginePower;
    float MAX_TURN_RATE;
    float MAX_BORESITE;
    float DEFAULT_DRAG;
    bool isActiveGuided;
        

    Rigidbody rigidbody;
    Transform seeker;
    Quaternion seekerDir;
    // Start is called before the first frame update
    void Awake()
    {
        seeker = transform.GetChild(3).transform;
        seekerDir = seeker.transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
    }

    Transform countermeasureManager;
    private void Start()
    {
        countermeasureManager = CountermeasureManager.instance.transform;      
    }

    GameObject targetObject;
    GameObject originTerget;

    [SerializeField] ParticleSystem burnEffect01, burnEffect02;
    [SerializeField] ParticleSystem explosionEffect;

    public void Init(Rigidbody rigidbody_, GameObject target_, MissileData missileData)
    {
        rigidbody.velocity = rigidbody_.velocity - this.transform.up * 10;
        targetObject = target_;
        originTerget = target_;

        lifeTime = missileData.lifeTime;
        burnTime = missileData.firstBurnTime;
        secondBurnTime = missileData.secondBurnTime;
        MAX_G = missileData.MAX_G;
        enginePower = missileData.enginePower;
        MAX_TURN_RATE = missileData.MAX_TURN_RATE;
        MAX_BORESITE = missileData.MAX_BORESITE;
        DEFAULT_DRAG = missileData.defaultDrag;
        if(missileData.seekerType == 1)
        {
            isActiveGuided = false;
        }
        else
        {
            isActiveGuided = true;
        }

        InvokeRepeating("CountermeasureCheck", 1, 0.5f);
        InvokeRepeating("RandumMissileWarningCount", 1, 0.5f);
        

        if (!isActiveGuided)
        {
            InvokeRepeating("NewTargetCheck", 1, 0.5f);
        }
        loftTime = 1;
    }

    float activeTime = 0;
    bool isBurn = true;
    bool isSecondBurn = false;
    float airPressure;

    // Update is called once per frame
    void FixedUpdate()
    {
        //sDebug.Log(rigidbody.velocity.magnitude);

        airPressure = Mathf.Pow(1 - ((this.transform.position.y / 300) / 145.45f), 5.2561f);
        rigidbody.drag = DEFAULT_DRAG * airPressure * ((rigidbody.velocity.magnitude + 700) / 700);

        seeker.transform.rotation = seekerDir;

        if (isBurn)
        {
            rigidbody.AddForce(this.transform.forward * enginePower * 0.5f, ForceMode.Force);
            if(!isSecondBurn)
            {
                rigidbody.AddForce(this.transform.forward * enginePower * 0.5f, ForceMode.Force);
            }
        }

        activeTime += Time.deltaTime;
        if(activeTime > burnTime && !isSecondBurn)
        {
            isSecondBurn = true;
        }

        if (activeTime > burnTime + secondBurnTime && isBurn)
        {
            isBurn = false;
            burnEffect01.Stop();
            burnEffect02.Stop();
        }
        if (activeTime > lifeTime + 10)
        {
            Destroy(this.gameObject);
        }

        sideForce = (this.transform.forward - rigidbody.velocity.normalized) * rigidbody.velocity.magnitude * rigidbody.velocity.magnitude * LIFT_POWER_VALUE;
        rigidbody.AddForce(Vector3.ClampMagnitude(sideForce, MAX_G), ForceMode.Force);

        if (targetObject != null && activeTime >= 10/MAX_TURN_RATE && activeTime < lifeTime)
        {
            Guided();
        }
    }

    Vector3 targetVec;

    float distance;

    Vector3 angleError_temp;
    Vector3 angleError_diff;

    Vector3 orderTemp = Vector3.zero;
    float orderX;
    float orderY;
    float orderXDiff;
    float orderYDiff;
    const float Kp = 3f;
    const float Kd = 10f;
    const float LIFT_POWER_VALUE = 0.002f;
    const float SPEED_GAIN = 900;
    Vector3 sideForce;

    float loftTime = 7;

    void Guided()
    {
        targetVec = targetObject.transform.position;       

        Vector3 toTargetVec = targetVec - this.transform.position;//missile to target Vector
        
        toTargetVec = toTargetVec.normalized;
        angleError_diff = toTargetVec - angleError_temp;//미분항
        angleError_temp = toTargetVec;


        Vector3 diffedAE = angleError_diff * SPEED_GAIN;//시선각 변화량

        Vector3 dieedAE_diff = diffedAE - orderTemp;
        orderTemp = diffedAE;
        Vector3 pnOrderVec = diffedAE * Kp + dieedAE_diff * Kd;//비례항법식


        Vector3 side1 = toTargetVec;
        Vector3 side2 = side1 + pnOrderVec;
        if(loftTime > 0 && (targetVec - this.transform.position).magnitude > 20000)
        {
            loftTime -= Time.deltaTime;
            side2 = side1 + new Vector3(0, 10, 0);
        }
        Vector3 orderAxisPn = Vector3.Cross(side1, side2);

        Vector3 orderAxis = orderAxisPn;

        if (sideForce.magnitude < MAX_G)
        {
            rigidbody.AddTorque(Vector3.ClampMagnitude(orderAxis * (rigidbody.velocity.magnitude * 0.0001f)
                * MAX_TURN_RATE, MAX_TURN_RATE * rigidbody.drag * 10), ForceMode.Force);
        }

        if (Vector3.Angle(this.transform.forward, targetVec - this.transform.position) > MAX_BORESITE)
        {
            targetObject = null;
            this.transform.SetParent(null);
        }
    }

    float countermeasureSensitivity = 1f;
    void CountermeasureCheck()
    {
        if (countermeasureManager == null)
        {
            targetObject = originTerget;
            return;
        }

        for(int i = 0; i < countermeasureManager.childCount; i++)
        {
            GameObject countermeasure = countermeasureManager.GetChild(i).gameObject;
            float countermeasureAngle = Vector3.Angle(this.transform.forward, countermeasure.transform.position - this.transform.position);
            if (Vector3.Angle(this.transform.forward, targetVec - this.transform.position) * countermeasureSensitivity > countermeasureAngle)
            {
                targetObject = countermeasure;
            }
        }
    }
    void NewTargetCheck()
    {
        originTerget = Rader.rader.target;
        targetObject = originTerget;
    }
    void RandumMissileWarningCount()//적이 미사일을 랜덤으로 확인하게 하는 함수
    {
        if (targetObject == null || targetObject.GetComponent<EnemyAiControl>() == null)
            return;


        float randomNum = Random.Range(0f, 1f);

        if (randomNum >= (targetVec - this.transform.position).magnitude * 0.0001f + (100 - GameManager.instance.difficulty) * 0.01f)
        {
            //Debug.Log((targetVec - this.transform.position).magnitude * 0.0001f);
            targetObject.GetComponent<EnemyAiControl>().MissileCheck(this.transform.position);
        }
    }

    public Minimap_Sprite missileMisimapSprite;

    private void OnTriggerEnter(Collider other)
    {
        targetObject = null;
        this.transform.SetParent(null);

        activeTime = burnTime;
        isBurn = false;
        rigidbody.velocity = Vector3.zero;
        missileMisimapSprite.gameObject.SetActive(false);

        burnEffect01.Stop();
        burnEffect02.Stop();
        explosionEffect.Play();

        Destroy(this.GetComponent<Collider>());

        if(other.gameObject.layer == 8)
        {
            other.GetComponent<Enemy>().Hit(100, false);
        }
        if(this.gameObject.layer == 9)
        {
            GetComponent<AudioSource>().Play();
        }
        if(other.gameObject.layer == 6)
        {
            PlayerInfo.playerInfo.Hit(100);
        }
        //rigidbody.velocity = Vector3.zero;
    }
}
