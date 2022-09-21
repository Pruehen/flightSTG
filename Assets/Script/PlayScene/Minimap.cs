using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject minimapCam;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerInfo.playerInfo.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetFwdPos = target.position + new Vector3(target.forward.x, 0, target.forward.z) * 1000;

        minimapCam.transform.position = targetFwdPos + new Vector3(0, 10000, 0);
        minimapCam.transform.eulerAngles = new Vector3(90, target.eulerAngles.y, 0);
    }
}
