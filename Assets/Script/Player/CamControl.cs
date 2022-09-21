using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    Quaternion targetRotarion= Quaternion.identity;
    const float DEFAULT_FOV = 75;
    const float ZOOM_FOV = 40;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotarion = Quaternion.Euler(PlayerInfo.playerInfo.pitchAxis * 8, PlayerInfo.playerInfo.yawAxis * 5, PlayerInfo.playerInfo.rollAxis * 15);
        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, targetRotarion, Time.deltaTime);

        if (isZooming)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, ZOOM_FOV, 0.1f);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, DEFAULT_FOV, 0.1f);
        }    
    }

    public bool isZooming= false;
}
