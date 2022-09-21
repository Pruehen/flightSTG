using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Sprite : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(90, this.transform.parent.transform.eulerAngles.y, 0);
    }
}
