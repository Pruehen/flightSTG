using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSurfacesControl : MonoBehaviour
{
    //public Texture2D tex;
    public Transform rightElevator, leftElevator;
    public Transform rightFlap, leftFlap;
    public Transform rightRudder, leftRudder;

    public float MRx, MRy, MRz;
    public float maxAngle, flapMaxAngle, rudderMaxAngle;
    public float[] heading = new float[6];
    public float[] bank = new float[6];
    public float rightElevatorSetAngle, leftElevatorSetAngle;
    public bool useMouse;
    public bool turnFlap;
    private bool f;
    public float turnSpeed;
    private float angleX;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        /*if (useMouse)
	    {
	        MRx = Mathf.Clamp(MRx + Input.GetAxis("Mouse Y")*0.05f, -1f, 1f);
	        MRz = Mathf.Clamp(MRz - Input.GetAxis("Mouse X")*0.05f, -1f, 1f);
	        MRy = Mathf.Clamp(MRy + Input.GetAxis("Yaw")*0.05f, -1f, 1f);
	    }
	    else
	    {
            MRx = Mathf.Clamp(MRx + Input.GetAxis("Vertical") * 0.05f, -1f, 1f);
            MRz = Mathf.Clamp(MRz - Input.GetAxis("Horizontal") * 0.05f, -1f, 1f);
            MRy = Mathf.Clamp(MRy + Input.GetAxis("Yaw") * 0.05f, -1f, 1f);   
	    }*/
        MRx = Mathf.Clamp(PlayerInfo.playerInfo.pitchAxis, -1, 1);
        MRz = PlayerInfo.playerInfo.rollAxis;
        MRy = -PlayerInfo.playerInfo.yawAxis;

        if (!turnFlap)
        {
            if (Input.GetKeyDown(KeyCode.P)) { turnFlap = true; f = false; }
        }
        else
        {
            if (turnFlap)
            {
                if (Input.GetKeyDown("p")) { turnFlap = false; f = true; }
            }
        }
        //rightElevator.transform.localRotation = Quaternion.Euler(new Vector3((MRz + MRx) * maxAngle, rightElevatorSetAngle, 0));
        //leftElevator.transform.localRotation = Quaternion.Euler(new Vector3((-MRz + MRx) * maxAngle, leftElevatorSetAngle, 0));
        TransAng(rightElevator, heading[0], (MRz - MRx) * maxAngle, bank[0]);
        TransAng(leftElevator, heading[1], (-MRz - MRx) * maxAngle, bank[1]);

        TransAng(rightRudder, heading[4], MRy * rudderMaxAngle, bank[4]);
        if(leftRudder != null)
        {
            TransAng(leftRudder, heading[5], MRy * rudderMaxAngle, bank[5]);
        }


	    if (turnFlap)
	    {
            angleX = Mathf.Lerp(angleX, 1, turnSpeed * Time.deltaTime);
            TransAng(rightFlap, heading[2], angleX * flapMaxAngle, bank[2]);
            TransAng(leftFlap, heading[3], angleX * flapMaxAngle, bank[3]);
	    }
	    else
	    {
            angleX = Mathf.Lerp(angleX, 0, turnSpeed * Time.deltaTime);
            TransAng(rightFlap, heading[2], angleX * flapMaxAngle, bank[2]);
            TransAng(leftFlap, heading[3], angleX * flapMaxAngle, bank[3]);
	    }
	}
    float c1, c2, c3, s1, s2, s3;
    float w, x, y, z;
    void TransAng(Transform tr, float heading, float attitude, float bank)
    {
        c1 = Mathf.Cos(heading * Mathf.Deg2Rad / 2);
        s1 = Mathf.Sin(heading * Mathf.Deg2Rad / 2);
        c2 = Mathf.Cos(attitude * Mathf.Deg2Rad / 2);
        s2 = Mathf.Sin(attitude * Mathf.Deg2Rad / 2);
        c3 = Mathf.Cos(bank * Mathf.Deg2Rad / 2);
        s3 = Mathf.Sin(bank * Mathf.Deg2Rad / 2);

        w = c1 * c2 * c3 - s1 * s2 * s3;
        x = s2 * c1 * c3 + s1 * c2 * s3;
        y = s1 * c2 * c3 + s2 * c1 * s3;
        z = s3 * c1 * c2 - s1 * s2 * c3;

        tr.transform.localRotation = new Quaternion(x, y, z, w);
    }
    /*void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width * 0.5f * (-MRz + 1f) - 8, Screen.height * 0.5f * (-MRx + 1f) - 8, 16f, 16f), tex);
    }*/
}
