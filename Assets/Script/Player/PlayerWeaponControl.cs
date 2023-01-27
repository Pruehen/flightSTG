using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    float gunShotDelay = 0.05f;
    float gunDelay;

    float maxMissileCool;

    PlayerSoundManager playerSoundManager;
    enum MissileIndex
    {
        Aim_9,
        MICA
    }

    Rigidbody rigidbody;

    MissileData haveMissileData;

    public static PlayerWeaponControl instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < INTED_MISSILE_COUNT; i++)
        {
            isCanFireMissile[i] = true;
            missileCooldown[i] = 0;
        }

        gunMuzzleEffect = gun.transform.GetChild(0).GetComponent<ParticleSystem>();
        rigidbody = this.GetComponent<Rigidbody>();
        playerSoundManager = this.GetComponent <PlayerSoundManager>();

        haveMissileData = new MissileData(8, 5.3f, 0, 600, 160, 60, 180, 0.05f, 0);
    }

    bool isFireing = false;

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

        /*if (Input.GetMouseButtonDown(1) && Rader.rader.target != null)
        {
            FireMissile(MissileIndex.Aim_9);
        }*/


        gunDelay += Time.deltaTime;
        if (isFireing && gunDelay >= gunShotDelay)
        { 
            BulletPool.instance.GetBullet(gun.transform.position, gun.transform.rotation, rigidbody.velocity);
            gunDelay = 0;            
        }

        /*if(Input.GetMouseButtonDown(0))
        {
            isFireing = true;
            gunMuzzleEffect.Play();
            playerSoundManager.gunSound.Play();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isFireing = false;
            gunMuzzleEffect.Stop();
            playerSoundManager.gunSound.Stop();
        }*/
    }

    public void FireMissile()
    {
        Vector3 firePoint = Vector3.zero;
        int index;

        GameObject selectedMissile = aim9_Model;
        /*switch(haveMissileData.bodyType)
        {
            case 0:
                selectedMissile = r60_model;
                break;
            case 1:
                selectedMissile = aim9_Model;
                break;
            case 2:
                selectedMissile = mica_model;
                break;
            case 3:
                selectedMissile = aim120_model;
                break;
            case 4:
                selectedMissile = big_r60_model;
                break;
            default:
                return;                
        }*/

        if(true)//missile == MissileIndex.Aim_9)
        {
            index = 0;

            firePoint = FirePointSet(index);
            if(firePoint == Vector3.zero)
            {
                index = 5;
                firePoint = FirePointSet(index);
            }      
            if(firePoint == Vector3.zero)
            {
                return;
            }

            Missile firedMissile = Instantiate(selectedMissile, firePoint, this.transform.rotation).GetComponent<Missile>();
            firedMissile.Init(GetComponent<Rigidbody>(), Rader.rader.target, haveMissileData);
            missilePoint[index].gameObject.SetActive(false);
            isCanFireMissile[index] = false;
            missileCooldown[index] = 4;
            maxMissileCool = 4;
        }

        return;
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
