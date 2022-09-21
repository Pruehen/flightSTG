using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        if (incommingMissile)
        {
            MissileWarning();
        }
    }

    public bool incommingMissile = false;
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
}
