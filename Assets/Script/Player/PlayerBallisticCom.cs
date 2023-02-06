using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallisticCom : MonoBehaviour
{
    public Rader PlayerRader;

    public GameObject gunSite_UI;
    public GameObject aimPoint_UI;

    public float bulletVelocity = 1000;

    // Start is called before the first frame update
    void Start()
    {
        gunSite_UI.SetActive(false);
    }

    // Update is called once per frame

    bool rangeOnTarget = false;
    void Update()
    {
        if(PlayerRader.target != null)
        {
            if (PlayerRader.targetDistance < 1500 && !rangeOnTarget)
            {
                rangeOnTarget = true;
                gunSite_UI.SetActive(true);
                aimPoint_UI.SetActive(true);
            }

            Vector3 leadPosition = PlayerRader.target.transform.position + PlayerRader.targetMoveVec * PlayerRader.targetDistance/(bulletVelocity+PlayerRader.relativeVelocity);

            Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position + this.transform.forward * PlayerRader.targetDistance);
            viewPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f, 0);
            gunSite_UI.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos);

            Vector3 viewPos2 = Camera.main.WorldToViewportPoint(leadPosition);
            viewPos2 = new Vector3(viewPos2.x - 0.5f, viewPos2.y - 0.5f, 0);
            aimPoint_UI.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos2);

            if((gunSite_UI.transform.localPosition - aimPoint_UI.transform.localPosition).magnitude < 50 && PlayerRader.targetDistance < 1500)
            {
                AutoFire();
            }
        }


        if(PlayerRader.targetDistance > 1500 || PlayerRader.target == null)
        {
            rangeOnTarget = false;
            gunSite_UI.SetActive(false);
            aimPoint_UI.SetActive(false);
        }
    }

    void AutoFire()
    {
        PlayerWeaponControl.instance.isFireing = true;
    }
}
