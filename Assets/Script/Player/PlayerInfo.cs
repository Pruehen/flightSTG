using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    float dc = 0.03f;//�׷°��
    float airPressure = 1;

    public float enginePower { get; set; }//0~1
    const float MAX_POWER = 16f;

    public float pitchAxis { get; set; }//-1~1
    const float MAX_PITCHMOMENT = 30;
    public float rollAxis { get; set; }//-1~1
    const float MAX_ROLLMOMENT = 80;
    public float yawAxis { get; set; }//-1~1
    const float MAX_YAWMOMENT = 5;

    const float MAX_LIFTPOWER = 40f;

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

        STALL_AOA = 20;
        wL = 274;//�͸�����
    }

    bool isStall = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        airPressure = Mathf.Pow(1 - ((this.transform.position.y / 300) / 145.45f), 5.2561f);
        rigidbody.drag = dc * airPressure * ((rigidbody.velocity.magnitude + 700) / 700);


        rigidbody.AddForce(this.transform.forward * enginePower * MAX_POWER * EnginePowerSet(), ForceMode.Force);
        if (MAX_LIFTPOWER < Mathf.Abs(liftPower))//���G�� ���� ��Ī���Ʈ ����
        {
            pitchAxis *= (MAX_LIFTPOWER / Mathf.Abs(liftPower));
        }
        //Debug.Log(pitchAxis);
        float speedValue = Mathf.Clamp(((rigidbody.velocity.magnitude - 30) * airPressure) * 0.003f, 0, 1);//�ӵ��� �ݺ���� ȸ�����Ʈ ����
        float aoaValue = 1;
        if (aoa > STALL_AOA)
        {
            aoaValue = Mathf.Clamp(STALL_AOA / aoa, 0, 1);
        }
        if(pitchAxis > 0)//��ġ �ٿ�
        {
            rigidbody.AddTorque(this.transform.right * pitchAxis * speedValue * aoaValue * 0.15f * MAX_PITCHMOMENT, ForceMode.Force);
        }
        else//��ġ ��
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

    [SerializeField] TextMeshProUGUI speedText, heightText;

    private void Update()
    {
        speedText.text = (int)(rigidbody.velocity.magnitude * 3.6f) + "km/h";
        heightText.text = (int)(this.transform.position.y) + "m";
    }

    public float aoa { get; private set; }
    float sideSlip;
    Vector3 aoaVec;
    public float STALL_AOA { get; private set;}
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
    float wL;//�͸����� (t/m^2)

    void LiftPowerSet()//�������� ���簢�� ���� ��� ����
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}