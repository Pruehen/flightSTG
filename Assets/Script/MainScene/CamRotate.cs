using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position += new Vector3(Random.Range(-10000, 10000), 0, Random.Range(-10000, 10000));
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(this.transform.up, Time.deltaTime);
    }
}
