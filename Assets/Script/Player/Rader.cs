using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rader : MonoBehaviour
{
    const float RADER_FOV = 70;
    public GameObject targetBox;
    public GameObject enemyManager;
    public TextMeshProUGUI distanceTmp;
    public TextMeshProUGUI velocityTmp;

    public GameObject radarBox;
    const int RADARBOX_X = 400;
    const int RADARBOX_Y = 300;


    //public GameObject radarTargetBox;

    public static Rader rader;
    private void Awake()
    {
        rader = this;
    }

    public GameObject target { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RaderSearch", 1, 0.2f);
    }



    // Update is called once per frame
    void Update()
    {
        //RadarBoxSet();

        if (target != null)
        {
            targetBox.SetActive(true);
            Vector3 viewPos = Camera.main.WorldToViewportPoint(target.transform.position);
            viewPos = new Vector3(viewPos.x-0.5f, viewPos.y-0.5f, 0);
            targetBox.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos);

            targetDistance = (target.transform.position - this.transform.position).magnitude;


            targetAngle = this.transform.InverseTransformDirection(target.transform.position - this.transform.position).normalized;
            targetMoveVec = target.GetComponent<Rigidbody>().velocity;
            relativeVelocity = PlayerInfo.playerInfo.speed - this.transform.InverseTransformDirection(targetMoveVec).z;

            distanceTmp.text = ((int)Vector3.Magnitude(target.transform.position - this.transform.position)).ToString();
            velocityTmp.text = (int)relativeVelocity + "m/s";

            TargetForget();
        }
        else
        {
            targetBox.SetActive(false);
        }
    }
    public float TargetDopplerLv()
    {
        return targetDistance / (Mathf.Abs(this.transform.InverseTransformDirection(targetMoveVec).z * 0.003f) + 200);
    }

    float targetPriority = float.MaxValue;
    public float targetDistance = 0;
    public Vector3 targetAngle;

    public Vector3 targetMoveVec;
    public float relativeVelocity;

    void RaderSearch()
    {
        for(int i = 0; i < enemyManager.transform.childCount; i++)
        {
            GameObject temp = enemyManager.transform.GetChild(i).gameObject;
            float tempPriority;
            tempPriority = ((temp.transform.position - this.transform.position).magnitude * 1) * (Vector3.Angle(this.transform.forward, (temp.transform.position - this.transform.position)));
            if(this.transform.InverseTransformDirection(temp.transform.position - this.transform.position).z < 0)
            {
                tempPriority = float.MaxValue;
            }

            if (targetPriority > tempPriority)
            {
                target = temp;
                targetPriority = tempPriority;
            }        
        }
        TargetForget();
    }

    void TargetForget()
    {
        if(target == null)
        {
            targetPriority = float.MaxValue;
            return;
        }
        if (RADER_FOV / 2 < Vector3.Angle(this.transform.forward, target.transform.position - this.transform.position))
        {
            target = null;
            targetPriority = float.MaxValue;
            targetBox.SetActive(false);
        }
    }

    public Vector3 ReturnTargetVec()
    {
        if(target != null)
        {
            Vector3 targetVec = target.transform.position - this.transform.position;
            targetVec = this.transform.InverseTransformDirection(targetVec).normalized;
            return targetVec;
        }
        return Vector3.zero;
    }

    void RadarBoxSet()
    {
        /*if (target == null)
        {
            radarTargetBox.SetActive(false);
            if (barDirection)
            {
                radarBarPosition_X += 4;
                if (radarBarPosition_X >= RADARBOX_X * 0.5f)
                    barDirection = false;
            }
            else
            {
                radarBarPosition_X -= 4;
                if (radarBarPosition_X <= -400 * 0.5f)
                    barDirection = true;
            }
            radarBar.transform.localPosition = new Vector3(radarBarPosition_X, 0, 0);
        }
        else
        {
            radarTargetBox.SetActive(true);
            float rtBoxPos_Y = Mathf.Pow(targetDistance, 0.55f);
            radarTargetBox.transform.localPosition = new Vector3(targetAngle.x * 200, rtBoxPos_Y-100, 0);
            radarBar.transform.localPosition = new Vector3(radarTargetBox.transform.localPosition.x, 0, 0);
        }*/
    }
}
