using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI;

public class PlayerWeaponControl : MonoBehaviour
{
    public GameObject r60_model, aim9_Model, mica_model, aim120_model, big_r60_model;
    public GameObject gun;
    ParticleSystem gunMuzzleEffect;

    const int INTED_MISSILE_COUNT = 6;
    public Transform[] missilePoint = new Transform[INTED_MISSILE_COUNT];
    float[] missileCooldown = new float[INTED_MISSILE_COUNT];
    bool[] isCanFireMissile = new bool[INTED_MISSILE_COUNT];

    float gunShotDelay = 0.02f;
    float gunDelay;

    float maxMissileCool;

    PlayerSoundManager playerSoundManager;

    Rigidbody rigidbody;

    public MissileData[] haveMissileDatas = new MissileData[INTED_MISSILE_COUNT];
    MissileData[] intedMissileDatas = new MissileData[INTED_MISSILE_COUNT];
    MissileData useMissileData;

    public static PlayerWeaponControl instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        haveMissileDatas[0] = new MissileData(8, 5.3f, 0, 300, 160, 30, 60, 0.05f, 0, 6000, "AIM-9M");
        haveMissileDatas[1] = new MissileData(25, 4f, 11, 200, 120, 30, 60, 0.03f, 1, 40000, "AIM-7M");

        for (int i = 0; i < INTED_MISSILE_COUNT; i++)
        {
            isCanFireMissile[i] = true;
            missileCooldown[i] = 0;
        }

        gunMuzzleEffect = gun.transform.GetChild(0).GetComponent<ParticleSystem>();
        rigidbody = this.GetComponent<Rigidbody>();
        playerSoundManager = this.GetComponent <PlayerSoundManager>();

        intedMissileDatas[0] = haveMissileDatas[0];
        intedMissileDatas[1] = haveMissileDatas[1];
        intedMissileDatas[2] = haveMissileDatas[1];
        for(int i = (int)(INTED_MISSILE_COUNT*0.5f); i < INTED_MISSILE_COUNT; i++)
        {
            intedMissileDatas[i] = intedMissileDatas[INTED_MISSILE_COUNT - i - 1];
        }

        useMissileData = intedMissileDatas[0];

        missileSeeker.gameObject.SetActive(false);
    }

    public bool isFireing = false;

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < INTED_MISSILE_COUNT; i++)
        {
            if(!isCanFireMissile[i])
            {
                missileCooldown[i] -= Time.deltaTime;

                if(missileCooldown[i] <= 0)
                {
                    isCanFireMissile[i] = true;
                    missilePoint[i].gameObject.SetActive(true);
                }
            }
        }

        gunDelay += Time.deltaTime;

        if (!isFireing)
        {
            gunMuzzleEffect.Stop();
            playerSoundManager.gunSound.Stop();
        }

        if (isFireing && gunDelay >= gunShotDelay)
        { 
            BulletPool.instance.GetBullet(gun.transform.position, gun.transform.rotation, rigidbody.velocity);
            gunDelay = 0;
            isFireing = false;

            gunMuzzleEffect.Play();
            playerSoundManager.gunSound.Play();
        }

        if(seekerOn)
        {
            float seeLv = 0;
            Vector3 viewPos;
            if (Rader.rader.target != null)
            {
                if (useMissileData.seekerType == 0)
                {
                    seeLv = useMissileData.sensitivity - Vector3.Magnitude(Rader.rader.target.GetComponent<Enemy>().heatLv() - this.transform.position);
                }
                else if(useMissileData.seekerType == 1)
                {
                    seeLv = useMissileData.sensitivity - Rader.rader.TargetDopplerLv();
                }

                viewPos = Camera.main.WorldToViewportPoint(Rader.rader.target.transform.position);
            }
            else
            {
                seeLv = 0;
                viewPos = Camera.main.WorldToViewportPoint(this.transform.position + this.transform.forward * 1000);
            }

            viewPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f, 0);
            missileSeeker.transform.localPosition = Camera.main.ViewportToScreenPoint(viewPos);

            if (seeLv > 0 && seekerLock == false)
            {
                seekerLock = true;
                playerSoundManager.missileSeekerSound.pitch = 2;
                missileSeeker.color = Color.red;
            }
            else if(seeLv <= 0 && seekerLock == true)
            {
                seekerLock = false;
                playerSoundManager.missileSeekerSound.pitch = 1;
                missileSeeker.color = Color.green;
            }
        }
    }

    public Image missileSeeker;
    public void SeekerToggle(bool value)
    {
        seekerOn = value;
        missileSeeker.gameObject.SetActive(value);
        if (useMissileData.seekerType == 0)
        {
            if (value)
            {
                playerSoundManager.missileSeekerSound.Play();
            }
            else
            {
                playerSoundManager.missileSeekerSound.Stop();
            }
        }
    }

    bool seekerOn = false;
    bool seekerLock = false;

    int useMissileNum = 0;
    public void MissileSwich()
    {
        useMissileNum++;

        if(useMissileNum > 1)
        {
            useMissileNum = 0;
        }

        useMissileData = intedMissileDatas[useMissileNum];

        PlayerInfo.playerInfo.MslTextSet(useMissileData.missileName);
    }

    public void FireMissile()
    {
        if(!seekerLock)
        {
            return;
        }

        Vector3 firePoint = Vector3.zero;
        int index;

        GameObject selectedMissile = aim9_Model;

        index = 0;

        firePoint = FirePointSet(index);
        if (firePoint == Vector3.zero)
        {
            index = 5;
            firePoint = FirePointSet(index);
        }
        if (firePoint == Vector3.zero)
        {
            return;
        }

        Missile firedMissile = Instantiate(selectedMissile, firePoint, this.transform.rotation).GetComponent<Missile>();
        firedMissile.Init(GetComponent<Rigidbody>(), Rader.rader.target, useMissileData);
        missilePoint[index].gameObject.SetActive(false);
        isCanFireMissile[index] = false;
        missileCooldown[index] = 4;
        maxMissileCool = 4;
    }

    Vector3 FirePointSet(int index)
    {
        if(missilePoint[index].gameObject.activeSelf)
        {
            return missilePoint[index].position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
