using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    static public BulletPool instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject bullet;

    private void Start()
    {
        for(int i = 0; i < 200; i++)
        {
            GameObject item = Instantiate(bullet, this.transform);
            item.SetActive(false);
        }
    }

    int getterIndex = 0;
    public Bullet GetBullet(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        getterIndex++;
        if (getterIndex >= 200)
        {
            getterIndex = 0;
        }

        if (!this.transform.GetChild(getterIndex).gameObject.activeSelf)
        {
            GameObject item = this.transform.GetChild(getterIndex).gameObject;
            item.SetActive(true);
            Bullet bullet = item.GetComponent<Bullet>();
            bullet.Init(position, rotation, velocity);
            return bullet;
        }
        return null;
    }

    public void ReturnBullet(GameObject item)
    {
        item.gameObject.SetActive(false);
    }
}
