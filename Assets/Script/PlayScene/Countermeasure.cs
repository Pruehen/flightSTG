using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countermeasure : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 12);
    }

    public void Init(Vector3 velocity)
    {
        rb.velocity = velocity;

        rb.AddForce(this.transform.up * -30, ForceMode.Impulse);
        rb.AddForce(this.transform.right * Random.Range(-4, 4), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
