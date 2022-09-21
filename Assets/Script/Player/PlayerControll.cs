using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControll : MonoBehaviour
{
    [SerializeField] Scrollbar throttleBar, pitchBar, rollBar, yawBar;
    ControlSystem controlSystem = ControlSystem.mouse;
    // Start is called before the first frame update
    void Start()
    {
        controlSystem = ControlSystem.mouse;

        //playerCountermeasure = 
    }
    const int SCREEN_X = 1920;
    const int SCREEN_Y = 1080;

    enum ControlSystem
    {
        keybord,
        mouse
    }

    float high_G_TurnValue = 1;

    // Update is called once per frame
    void FixedUpdate()//조종면, 엔진 조작
    {
        #region 엔진축
        if (Input.GetKey(KeyCode.LeftShift) && throttleBar.value < 1)
        {
            throttleBar.value += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftControl) && throttleBar.value > 0)
        {
            throttleBar.value -= Time.deltaTime;
            if (throttleBar.value < 0.9f && GetComponent<Rigidbody>().velocity.magnitude < 400 && GetComponent<Rigidbody>().velocity.magnitude > 70)
            {
                high_G_TurnValue = 3f;
            }
            else
            {
                high_G_TurnValue = 1;
            }
        }
        else
        {
            high_G_TurnValue = 1;
        }
        PlayerInfo.playerInfo.enginePower = throttleBar.value;
        #endregion

        switch(controlSystem)
        {
            case ControlSystem.keybord:
                #region 피치축
                if (Input.GetKey(KeyCode.S))
                {
                    pitchBar.value = Mathf.Lerp(pitchBar.value, 0, 0.1f);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    pitchBar.value = Mathf.Lerp(pitchBar.value, 1, 0.1f);
                }
                else
                {
                    pitchBar.value = Mathf.Lerp(pitchBar.value, 0.5f, 0.1f);
                }

                #endregion

                #region 롤축
                if (Input.GetKey(KeyCode.Q))
                {
                    rollBar.value = Mathf.Lerp(rollBar.value, 0, 0.1f);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    rollBar.value = Mathf.Lerp(rollBar.value, 1, 0.1f);
                }
                else
                {
                    rollBar.value = Mathf.Lerp(rollBar.value, 0.5f, 0.1f);
                }

                #endregion
                break;
            case ControlSystem.mouse:
                if (Input.GetKey(KeyCode.S))
                {
                    pitchBar.value = Mathf.Lerp(pitchBar.value, 0, 0.1f);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    pitchBar.value = Mathf.Lerp(pitchBar.value, 1, 0.1f);
                }
                else
                {
                    pitchBar.value = Mathf.Lerp(pitchBar.value, ReturnMousePosition().y, 0.1f);
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    rollBar.value = Mathf.Lerp(rollBar.value, 0, 0.1f);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    rollBar.value = Mathf.Lerp(rollBar.value, 1, 0.1f);
                }
                else
                {
                    rollBar.value = Mathf.Lerp(rollBar.value, ReturnMousePosition().x, 0.1f);
                }
                break;
        }    

        #region 요축
        if (Input.GetKey(KeyCode.A))
        {
            yawBar.value = Mathf.Lerp(yawBar.value, 0, 0.05f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            yawBar.value = Mathf.Lerp(yawBar.value, 1, 0.05f);
        }
        else
        {
            yawBar.value = Mathf.Lerp(yawBar.value, 0.5f, 0.05f);
        }
        #endregion
        PlayerInfo.playerInfo.pitchAxis = Mathf.Clamp((pitchBar.value * 2 - 1) * high_G_TurnValue, -3f, 1);
        PlayerInfo.playerInfo.rollAxis = rollBar.value * 2 - 1;
        PlayerInfo.playerInfo.yawAxis = yawBar.value * 2 - 1;

    }

    public PlayerCountermeasure playerCountermeasure;
    private void Update()//기타 조작
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            myCam.isZooming = !myCam.isZooming;
        }

        if(Input.GetKeyDown("f"))
        {
            playerCountermeasure.isActiveCountermeasureSwitch();
        }
    }

    Vector3 ReturnMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = new Vector3(mousePosition.x / SCREEN_X, mousePosition.y / SCREEN_Y, 0);

        return mousePosition;
    }

    public CamControl myCam;
}
