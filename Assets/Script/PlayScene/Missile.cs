using System.Collections;
using System.Collections.Generic;
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
        InvokeRepeating("CountermeasureCheck", 1, 0.5f);
    }

    GameObject targetObject;
    GameObject originTerget;

    [SerializeField] ParticleSystem burnEffect01, burnEffect02;
    [SerializeField] ParticleSystem explosionEffect;

    public void Init(Rigidbody rigidbody_, GameObject target_, MissileData missileData)
    {
        rigidbody.velocity = rigidbody_.velocity;
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
    }

    float activeTime = 0;
    bool isBurn = true;
    bool isSecondBurn = false;
    float airPressure;

    // Update is called once per frame
    void FixedUpdate()
    {
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

        Vector3 sideForce = (this.transform.forward - rigidbody.velocity.normalized) * rigidbody.velocity.magnitude * rigidbody.velocity.magnitude * LIFT_POWER_VALUE;
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
    const float Kp = 1f;
    const float Kd = 1f;
    const float LIFT_POWER_VALUE = 0.05f;
    const float SPEED_GAIN = 900;

    void Guided()
    {
        //Debug.Log(rigidbody.velocity.magnitude);
        //seeker.forward = rigidbody.velocity;
        targetVec = targetObject.transform.position;

        Vector3 angleError = targetVec - this.transform.position;//��Ŀ�� �ٶ󺸴� �� ����
        angleError = seeker.transform.InverseTransformDirection(angleError).normalized;//����ȭ + ����ȭ
        angleError_diff = angleError - angleError_temp;//�̺���
        angleError_temp = angleError;


        Vector3 orderVec = angleError_diff * SPEED_GAIN;

        if (angleError.z > 0)
        {
            orderVec = new Vector3(orderVec.x, orderVec.y, 0);
        }
        else
        {
            
        }


        orderXDiff = (orderVec - orderTemp).x;
        orderYDiff = (orderVec - orderTemp).y;

        //Debug.Log(orderVec);
        orderX = orderVec.x * Kp + orderXDiff * Kd;
        orderY = orderVec.y * Kp + orderYDiff * Kd;

        Debug.Log("x = " + orderX + "y = " + orderY);


        rigidbody.AddTorque(Vector3.ClampMagnitude(this.transform.right * -orderY, MAX_TURN_RATE), ForceMode.Force);
        rigidbody.AddTorque(Vector3.ClampMagnitude(this.transform.up * orderX, MAX_TURN_RATE), ForceMode.Force);

        if (Vector3.Angle(this.transform.forward, targetVec - this.transform.position) > MAX_BORESITE)
        {
            targetObject = null;
            this.transform.SetParent(null);
        }
    }

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
            if (Vector3.Angle(this.transform.forward, targetVec - this.transform.position) > countermeasureAngle)
            {
                targetObject = countermeasure;
            }
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
            other.GetComponent<Enemy>().Destroyed();
        }
        if(this.gameObject.layer == 9)
        {
            GetComponent<AudioSource>().Play();
        }
        //rigidbody.velocity = Vector3.zero;
    }
}