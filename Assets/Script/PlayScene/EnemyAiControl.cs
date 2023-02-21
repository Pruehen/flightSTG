using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAiControl : MonoBehaviour
{
    Enemy mainEnemy;
    Rigidbody rigidbody;

    Vector3 targetPosition;
    public void TargetPositionSet(Vector3 vector3)
    {
        targetPosition = vector3;
    }
    Transform target_Player = null;
    // Start is called before the first frame update
    void Start()
    {
        missileManager = GameObject.Find("EnemyActiveMissileManager");

        mainEnemy = GetComponent<Enemy>();
        thisCountermeasure = GetComponent<EnemyCountermeasure>();

        rigidbody = gameObject.GetComponent<Rigidbody>();
        targetPosition = new Vector3(0, 4000, 0);

        target_Player = PlayerInfo.playerInfo.transform;

        
    }

    State state = State.idle;
    // Update is called once per frame
    void FixedUpdate()
    {
        AiSet();

        missileTime -= Time.deltaTime;
    }



    enum State
    {
        idle,
        cruise,
        tracking,
        groundEvading,
        missileEvading
    }

    float groundCheckDistance = 2000;
    [SerializeField] LayerMask layerMask;

    bool missileView = false;
    Vector3 incomingMissileVec;
    public void MissileCheck(Vector3 missileVec)
    {
        if (missileView == false)
        {
            missileView = true;
            incomingMissileVec = missileVec;
        }
    }
    float missileEvadeCount = 0;

    void AiSet()
    {
        RaycastHit hit;        

        if (target_Player == null)
        {
            if (Vector3.Magnitude(this.transform.position - targetPosition) < 1000 && state != State.idle)
            {
                state = State.idle;
            }
            else if (Vector3.Magnitude(this.transform.position - targetPosition) >= 5000 && state != State.cruise)
            {
                state = State.cruise;
            }
        }
        else if (target_Player != null)
        {
            state = State.tracking;
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, groundCheckDistance, layerMask) || 
            Physics.Raycast(transform.position, transform.forward + transform.up * 0.2f, out hit, groundCheckDistance, layerMask) ||
            Physics.Raycast(transform.position, transform.forward - transform.up * 0.2f, out hit, groundCheckDistance, layerMask))
        {
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);
                state = State.groundEvading;
            }
            else
            {
                state = State.cruise;
            }
        }

        if(missileView)
        {
            state = State.missileEvading;
            missileEvadeCount += Time.deltaTime;
            if (missileEvadeCount >= 5)
            {
                missileView = false;
            }
        }

        //Debug.Log(state);
        switch (state)
        {
            case State.idle:
                ControlSet_Idle();
                break;
            case State.cruise:
                ControlSet_Cruise();
                break;
            case State.tracking:
                ControlSet_Tracking();
                break;
            case State.groundEvading:
                ControlSet_GroundEvading();
                break;
            case State.missileEvading:
                ControlSet_MissileEvading();
                break;
            default:
                break;
        }


    }
    Vector3 aimPoint = Vector3.forward;
    void ControlSet_Idle()
    {
        aimPoint = new Vector3(this.transform.up.y, -0.5f, Mathf.Clamp(rigidbody.velocity.y * 0.1f, -1, 1));

        mainEnemy.enginePower = 0.5f;
        AxisControl();
    }
    void ControlSet_Cruise()
    {
        Vector3 toTargetLocalVec = transform.InverseTransformDirection(targetPosition - this.transform.position);

        aimPoint = new Vector3(this.transform.right.y, Mathf.Clamp(-toTargetLocalVec.y * 0.01f, -1, 1), Mathf.Clamp(toTargetLocalVec.x, -1, 1));
        mainEnemy.enginePower = 0.5f;
        AxisControl();
    }

    public GameObject missile;
    GameObject missileManager;
    float missileTime = 5;
    public float missileCoolTime = 5f;
    void ControlSet_Tracking()
    {
        Vector3 toTargetVec = target_Player.position - this.transform.position;
        if (Vector3.Magnitude(toTargetVec) > 5000)
        {
            targetPosition = target_Player.position + new Vector3(0, 1000, 0);
            ControlSet_Cruise();
        }
        else
        {
            Vector3 toTargetLocalVec = transform.InverseTransformDirection(toTargetVec);
            if(toTargetLocalVec.z >= 0)
            {
                aimPoint = new Vector3(Mathf.Clamp(toTargetLocalVec.x * 0.01f, -1, 1), Mathf.Clamp(-toTargetLocalVec.y * 0.01f, -1, 1), Mathf.Clamp(toTargetLocalVec.x * 0.01f, -1, 1));
            }
            else
            {
                aimPoint = new Vector3(Mathf.Clamp(toTargetLocalVec.x, -1, 1), -1, 0);
            }


            mainEnemy.enginePower = 1;
            AxisControl();

            if (Vector3.Magnitude(toTargetVec) < 6000 && missileTime <= 0 && Vector3.Angle(this.transform.forward, toTargetVec) < 10)
            {
                missileTime = missileCoolTime;

                Missile firedMissile = Instantiate(missile, this.transform.position, this.transform.rotation, missileManager.transform).GetComponent<Missile>();
                firedMissile.Init(rigidbody, target_Player.gameObject, GetInstanceMissileData());
            }
        }
    }

    void ControlSet_GroundEvading()
    {
        RaycastHit upHit;

        aimPoint = new Vector3(this.transform.right.y, -1, 0);


        mainEnemy.enginePower = 1f;
        AxisControl();
    }

    EnemyCountermeasure thisCountermeasure;

    void ControlSet_MissileEvading()
    {
        Debug.Log("evading");
        Vector3 toTargetVec = incomingMissileVec - this.transform.position;
        Vector3 toTargetLocalVec = transform.InverseTransformDirection(toTargetVec);

        if(toTargetLocalVec.z >= 0)//미사일이 전방에 있음
        {
            aimPoint = new Vector3(Mathf.Clamp(toTargetLocalVec.z, -1, 1), Mathf.Clamp(-toTargetLocalVec.z, -1, 1), 0);
        }
        else if(toTargetLocalVec.z < 0)//미사일이 후방에 있음
        {
            aimPoint = new Vector3(Mathf.Clamp(-toTargetLocalVec.z, -1, 1), Mathf.Clamp(toTargetLocalVec.z, -1, 1), 0);
        }

        mainEnemy.enginePower = 2f;
        AxisControl();
        thisCountermeasure.SetActiveCountermeasureOnTrigger();
    }

    MissileData GetInstanceMissileData()//작동시간, 1차 부스터, 2차 부스터, 최대기동, 추력, 추적률, 시커각, 항력, 시커타입(0,1,2), 동체타입(0~4)
    {
        MissileData missileData = new MissileData(10, 5, 0, 180, 150, 20, 90, 0.08f, 0, 4000, "AIM-9H");

        return missileData;
    }


    void AxisControl()
    {
        mainEnemy.rollAxis = -aimPoint.x;
        mainEnemy.pitchAxis = aimPoint.y;
        mainEnemy.yawAxis = aimPoint.z;
    }
}
