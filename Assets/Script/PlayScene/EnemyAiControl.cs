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
        evading
    }

    float groundCheckDistance = 2000;
    [SerializeField] LayerMask layerMask;

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
                state = State.evading;
            }
            else
            {
                state = State.cruise;
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
            case State.evading:
                ControlSet_Evading();
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

    void ControlSet_Evading()
    {
        RaycastHit upHit;

        aimPoint = new Vector3(this.transform.right.y, -1, 0);


        mainEnemy.enginePower = 1f;
        AxisControl();
    }

    MissileData GetInstanceMissileData()//�۵��ð�, 1�� �ν���, 2�� �ν���, �ִ�⵿, �߷�, ������, ��Ŀ��, �׷�, ��ĿŸ��(0,1,2), ��üŸ��(0~4)
    {
        MissileData missileData = new MissileData();
        missileData.lifeTime = 10;
        missileData.firstBurnTime = 2;
        missileData.secondBurnTime = 8;
        missileData.MAX_G = 150;
        missileData.enginePower = 160;
        missileData.MAX_TURN_RATE = 10;
        missileData.MAX_BORESITE = 90;
        missileData.defaultDrag = 0.08f;
        missileData.seekerType = 2;
        missileData.bodyType = 2;
        return missileData;
    }


    void AxisControl()
    {
        mainEnemy.rollAxis = -aimPoint.x;
        mainEnemy.pitchAxis = aimPoint.y;
        mainEnemy.yawAxis = aimPoint.z;
    }
}