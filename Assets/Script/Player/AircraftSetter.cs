using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftSetter : MonoBehaviour
{
    private void Awake()
    {
        AircraftInit();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    GameObject playerObject;

    void AircraftInit()
    {
        string aircraftName = AircraftManager.instance.useStaticData.name;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).name == aircraftName)
            {
                playerObject = this.transform.GetChild(i).gameObject;
                playerObject.SetActive(true);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MissileFire()
    {
        PlayerWeaponControl.instance.FireMissile();
    }
    public void SeekerToggle(bool value)
    {
        PlayerWeaponControl.instance.SeekerToggle(value);
    }
    public void MissileSwich()
    {
        PlayerWeaponControl.instance.MissileSwich();
    }
    public void CountermeasureSet(bool value)
    {
        PlayerControll.instance.CounterMeasureSet(value);
    }
    public void AutoAimToggle()
    {
        PlayerControll.instance.isAutoAimToggle();
    }
    public void Calibration()
    {
        PlayerControll.instance.StartVecReset();
    }
}
