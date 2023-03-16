using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMWR : MonoBehaviour
{
    GameObject enemyMissileManager;

    private void Awake()
    {
        enemyMissileManager = GameObject.Find("EnemyActiveMissileManager");
    }
    // Start is called before the first frame update
    void Start()
    {
        arrow1.SetActive(false);
        arrow2.SetActive(false);
        arrow3.SetActive(false);

        arrowImage1 = arrow1.transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrowImage2 = arrow2.transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrowImage3 = arrow3.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    [SerializeField] GameObject mwrImage;

    // Update is called once per frame
    void Update()
    {
        if (enemyMissileManager.transform.childCount != 0)
        {
            incommingMissile = true;
        }
        else
        {
            incommingMissile = false;
            mwrImage.SetActive(false);
            arrow1.SetActive(false);
            arrow2.SetActive(false);
            arrow3.SetActive(false);
        }

        if (incommingMissile)
        {
            MissileWarning();

            ArrowDrow();
        }
    }


    bool incommingMissile = false;
    public void SetMwrActive(bool value)
    {
        incommingMissile = value;
    }

    float loopTime = 0;
    public AudioSource mwr;
    void MissileWarning()
    {
        loopTime += Time.deltaTime;
        if (loopTime >= 0 && loopTime <= Time.deltaTime)
        {
            mwrImage.SetActive(true);
            mwr.Play();
        }
        else if (loopTime >= 0.5f && loopTime < 1)
        {
            mwrImage.SetActive(false);
        }
        else if (loopTime >= 1)
        {
            loopTime = 0;
        }
    }

    [SerializeField]
    GameObject arrow1, arrow2, arrow3;
    SpriteRenderer arrowImage1, arrowImage2, arrowImage3;
    void ArrowDrow()
    {
        switch(enemyMissileManager.transform.childCount)
        {
            case 1:
                arrow1.SetActive(true);
                arrow2.SetActive(false);
                arrow3.SetActive(false);

                ArrowSet(enemyMissileManager.transform.GetChild(0).gameObject, arrow1, arrowImage1);
                break;
            case 2:
                arrow1.SetActive(true);
                arrow2.SetActive(true);
                arrow3.SetActive(false);

                ArrowSet(enemyMissileManager.transform.GetChild(0).gameObject, arrow1, arrowImage1);
                ArrowSet(enemyMissileManager.transform.GetChild(1).gameObject, arrow2, arrowImage2);

                break;
            default:
                arrow1.SetActive(true);
                arrow2.SetActive(true);
                arrow3.SetActive(true);

                ArrowSet(enemyMissileManager.transform.GetChild(0).gameObject, arrow1, arrowImage1);
                ArrowSet(enemyMissileManager.transform.GetChild(1).gameObject, arrow2, arrowImage2);
                ArrowSet(enemyMissileManager.transform.GetChild(2).gameObject, arrow3, arrowImage3);

                break;
        }
    }

    void ArrowSet(GameObject target, GameObject arrow, SpriteRenderer image)
    {
        Vector3 targetVec = this.transform.position - target.transform.position;
        targetVec = this.transform.InverseTransformDirection(targetVec);

        arrow.transform.localPosition = -targetVec.normalized * 10;
        arrow.transform.LookAt(target.transform.position);

        image.color = new Color(1, (targetVec.magnitude * 0.0005f), 0);
    }
}
