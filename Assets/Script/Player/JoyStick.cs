using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
{
    public GameObject stick;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private Touch tempTouchs;
    private Vector3 touchedPos;
    private bool touchOn;

    // Update is called once per frame
    void Update()
    {
        /*if(Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                stick.transform.position = Input.GetTouch(i).position;
                Debug.Log(Input.GetTouch(i).position);
                stick.transform.localPosition = new Vector3(Mathf.Clamp(stick.transform.localPosition.x, -100, 100), Mathf.Clamp(stick.transform.localPosition.y, -100, 100), 0);
            }
        }
        else if(Input.touchCount <= 0)
        {
            stick.transform.localPosition = Vector3.Lerp(stick.transform.localPosition, Vector3.zero, Time.deltaTime * 10);
        }*/
    }

    public Vector2 JoyPosition()
    {
        return new Vector2(stick.transform.localPosition.x + 100, stick.transform.localPosition.y + 100) * 0.005f;
    }
}
