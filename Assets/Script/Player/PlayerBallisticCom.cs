using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallisticCom : MonoBehaviour
{
    public Rader PlayerRader;

    public GameObject gunSite_UI;
    public GameObject aimPoint_UI;
    // Start is called before the first frame update
    void Start()
    {
        gunSite_UI.SetActive(false);
    }

    // Update is called once per frame

    bool rangeOnTarget = false;
    void Update()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position + this.transform.forward * 3000);
        viewPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f, 0);
        gunSite_UI.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos);

        if(PlayerRader.target != null)
        {
            if (PlayerRader.targetDistance < 3000 && !rangeOnTarget)
            {
                rangeOnTarget = true;
                gunSite_UI.SetActive(true);
                aimPoint_UI.SetActive(true);
            }

            Vector3 viewPos2 = Camera.main.WorldToViewportPoint(PlayerRader.target.transform.position + PlayerRader.target.transform.forward * PlayerRader.targetDistance * 0.25f);
            viewPos2 = new Vector3(viewPos2.x - 0.5f, viewPos2.y - 0.5f, 0);
            aimPoint_UI.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos2);
        }


        if(PlayerRader.targetDistance > 3000 || PlayerRader.target == null)
        {
            rangeOnTarget = false;
            gunSite_UI.SetActive(false);
            aimPoint_UI.SetActive(false);
        }
    }
}
