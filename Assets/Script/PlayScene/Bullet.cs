using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float spreed;    

    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }
    float airPressure;
    const float GLOBAL_DRAG = 0.1f;
    // Update is called once per frame

    const float MAX_SPREAD = 10;
    float spreadX = 0;
    float spreadY = 0;
    void Update()
    {
        airPressure = Mathf.Pow(1 - ((this.transform.position.y / 300) / 145.45f), 5.2561f);
        rigidbody.drag = GLOBAL_DRAG * airPressure * ((rigidbody.velocity.magnitude + 700) / 700);

        velocityIntg += rigidbody.velocity.magnitude * Time.deltaTime;

        if(velocityIntg > 3000)
        {
            InPool();
        }
    }

    float velocityIntg = 0;

    public void Init(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        this.transform.position = position;
        this.transform.rotation = rotation;

        spreadX = Random.Range(-MAX_SPREAD, MAX_SPREAD);
        spreadY = Random.Range(-MAX_SPREAD, MAX_SPREAD);

        rigidbody.velocity = velocity;
        rigidbody.AddForce(this.transform.forward * bulletSpeed, ForceMode.Impulse);
        rigidbody.AddForce(this.transform.up * spreadX, ForceMode.Impulse);
        rigidbody.AddForce(this.transform.right * spreadY, ForceMode.Impulse);
        velocityIntg = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().Hit(10, true);
        }
        InPool();
    }

    void InPool()
    {
        BulletPool.instance.ReturnBullet(this.gameObject);
    }
}
