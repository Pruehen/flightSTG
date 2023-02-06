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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemyCreate()
    {
        Vector3 createPosition = new Vector3(Random.Range(-30000, 30000), Random.Range(1000, 10000), Random.Range(-30000, 30000));
        Instantiate(enemy01, this.transform).transform.position = createPosition;
    }

    public void DebriCreate(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        GameObject debri = Instantiate(enemy01_Debri, position, rotation);
        debri.GetComponent<Rigidbody>().velocity = velocity;
    }
}
