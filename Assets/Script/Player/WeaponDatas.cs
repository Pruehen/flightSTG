using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatas : MonoBehaviour
{
    public static WeaponDatas instance;

    public static MissileData aim_9j = (new MissileData(8, 2.2f, 0, 200, 220, 16, 30, 0.07f, 0, 6000, "AIM-9J"));
    public static MissileData aim_9g = (new MissileData(8, 5.3f, 0, 180, 160, 16, 60, 0.05f, 0, 6000, "AIM-9G"));
    public static MissileData aim_9h = (new MissileData(8, 5.3f, 0, 180, 160, 20, 60, 0.05f, 0, 8000, "AIM-9H"));
    public static MissileData aim_9c = (new MissileData(8, 5.3f, 0, 180, 160, 16, 60, 0.05f, 1, 2000, "AIM-9C"));
    public static MissileData aim_9m = (new MissileData(8, 5.3f, 0, 300, 160, 24, 60, 0.05f, 0, 9000, "AIM-9M"));
    public static MissileData aim_7e = (new MissileData(12, 2.8f, 0, 250, 180, 15, 30, 0.03f, 1, 10000, "AIM-7E"));
    public static MissileData aim_7m = (new MissileData(25, 4f, 11, 250, 100, 15, 60, 0.03f, 1, 12000, "AIM-7M"));
    public static MissileData aim_54a = (new MissileData(50, 30, 0, 150, 50, 15, 120, 0.03f, 2, 50000, "AIM-54A"));

    private void Awake()
    {
        instance = this;

        datas.Add(aim_9j);
        datas.Add(aim_9g);
        datas.Add(aim_9h);
        datas.Add(aim_9c);
        datas.Add(aim_9m);
        datas.Add(aim_7e);
        datas.Add(aim_7m);
        datas.Add(aim_54a);
    }

    public List<MissileData> datas = new List<MissileData>();

    private void Start()
    {

    }

    public MissileData Find(string name)
    {
        foreach (MissileData item in datas)
        {
            if (item.missileName == name)
                return item;
        }

        return null;
    }

    public MissileData DefaultMissile()
    {
        return aim_9j;
    }
}
