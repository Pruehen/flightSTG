using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Target : MonoBehaviour
{
    public void Destroyed(Enemy value)
    {
        //EnemyManager.instance.EnemyCreate();

        EnemyManager.instance.DebriCreate(this.transform.position, this.transform.rotation, value.rigidbody.velocity);
        EnemyManager.instance.EnemyDestroy();

        MissionSceneManager.instance.ScoreUp(250);
        Destroy(this.gameObject);
    }

    public int score = 250;
}
