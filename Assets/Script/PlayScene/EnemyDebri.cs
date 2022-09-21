using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebri : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
        GetComponent<Rigidbody>().AddTorque(this.transform.forward * 360, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
