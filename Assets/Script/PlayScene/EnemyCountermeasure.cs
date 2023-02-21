using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountermeasure : MonoBehaviour
{
    public GameObject countermeasure;
    GameObject countermeasureManager;

    Transform dispensePoint_01;
    Enemy parentEnemy;
    //Transform dispensePoint_02;
    // Start is called before the first frame update
    void Start()
    {
        dispensePoint_01 = this.transform;
        parentEnemy = this.GetComponent<Enemy>();
        countermeasureManager = CountermeasureManager.instance.gameObject;
        //dispensePoint_02 = this.transform.GetChild(1);
    }

    bool isActiveCountermeasure = false;
    public void SetActiveCountermeasureOnTrigger()
    {
        if(isActiveCountermeasure == false)
            isActiveCountermeasure = true;
    }

    float dispenseTime = 0.15f;
    float timeDelay = 0;

    // Update is called once per frame
    void Update()
    {
        if (isActiveCountermeasure)
        {
            timeDelay += Time.deltaTime;

            if (timeDelay > dispenseTime)
            {
                timeDelay = 0;
                FlareDispense();
                isActiveCountermeasure = false;
            }
        }
    }

    void FlareDispense()
    {
        Countermeasure cm1 = Instantiate(countermeasure, dispensePoint_01.position, dispensePoint_01.rotation, countermeasureManager.transform).GetComponent<Countermeasure>();
        //Countermeasure cm2 = Instantiate(countermeasure, dispensePoint_02.position, dispensePoint_02.rotation).GetComponent<Countermeasure>();

        cm1.Init(parentEnemy.rigidbody.velocity);
        //cm2.Init(playerVelocity);
    }
}
