using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    static public EnemyManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject enemy01;
    public GameObject enemy01_Debri;

    bool infinityMod = false;
    // Start is called before the first frame update

    int createEnemyCount;
    int destroyEnemy = 0;
    void Start()
    {
        if(GameManager.instance.difficulty == -1)
        {
            infinityMod = true;
        }

        createEnemyCount = (int)GameManager.instance.difficulty + 2;
        for (int i = 0; i < Mathf.Clamp(10, 0, createEnemyCount); i++)
        {
            EnemyCreate();
        }
        createEnemyCount -= 10;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemyCreate()
    {
        float createRadius = 10000 + (int)GameManager.instance.difficulty * 1000;

        Vector3 createPosition = new Vector3(Random.Range(-createRadius, createRadius), Random.Range(1000, 10000), Random.Range(-createRadius, createRadius));
        Instantiate(enemy01, this.transform).transform.position = createPosition;
    }

    public void EnemyDestroy()
    {
        //Debug.Log(this.transform.childCount);
        destroyEnemy++;
        if (createEnemyCount > 0)
        {
            EnemyCreate();
            createEnemyCount--;
        }
        if(infinityMod)
        {
            GameManager.instance.DifficultySet(destroyEnemy + 1);
            EnemyCreate();
            EnemyCreate();
        }
        else if(this.transform.childCount == 1)
        {
            MissionSceneManager.instance.GameEnd(true);
        }
    }

    public void DebriCreate(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        GameObject debri = Instantiate(enemy01_Debri, position, rotation);
        debri.GetComponent<Rigidbody>().velocity = velocity;
    }
}
