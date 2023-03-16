using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountermeasure : MonoBehaviour
{
    public static PlayerCountermeasure instance;

    public GameObject countermeasure;
    public GameObject countermeasureManager;

    Transform dispensePoint_01;
    Transform dispensePoint_02;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        dispensePoint_01 = this.transform.GetChild(0);
        dispensePoint_02 = this.transform.GetChild(1);

        countermeasureCount = 200;
    }

    bool isActiveCountermeasure = false;
    public void SetActiveCountermeasureSwitch(bool value)
    {
        isActiveCountermeasure = value;
    }

    float dispenseTime = 0.15f;
    float timeDelay = 0;

    public int countermeasureCount { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (isActiveCountermeasure && countermeasureCount > 0)
        {
            timeDelay += Time.deltaTime;

            if (timeDelay > dispenseTime)
            {
                timeDelay = 0;
                FlareDispense();
            }
        }
    }

    void FlareDispense()
    {
        Vector3 playerVelocity = PlayerInfo.playerInfo.rigidbody.velocity;

        Countermeasure cm1 = Instantiate(countermeasure, dispensePoint_01.position, dispensePoint_01.rotation, countermeasureManager.transform).GetComponent<Countermeasure>();
        //Countermeasure cm2 = Instantiate(countermeasure, dispensePoint_02.position, dispensePoint_02.rotation).GetComponent<Countermeasure>();

        cm1.Init(playerVelocity);
        countermeasureCount--;

        PlayerInfo.playerInfo.FlrTextSet(countermeasureCount);
        //cm2.Init(playerVelocity);
    }
}
