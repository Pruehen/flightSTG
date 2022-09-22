using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectSystem : MonoBehaviour
{
    public static SelectSystem instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject[] selectTool = new GameObject[6];
    public TextMeshProUGUI missileInfoTmp;
    // Start is called before the first frame update

    int selectedGroup;
    public void SelectButtonClick(int index)
    {
        selectedGroup = index;
        for (int i = 0; i < selectTool.Length; i++)//index = 0~5
        {
            if(i == index)
            {
                selectTool[i].SetActive(true);
            }
            else
            {
                selectTool[i].SetActive(false);
            }
        }
    }

    public void SelectPartButtonClick(int index)//index = 0~4
    {
        switch (selectedGroup)
        {
            case 0://시커
                newMissile.SeekerSet(index);
                switch (index)
                {
                    case 0://열추적
                        newMissile.boresiteGain = 0.66f;
                        break;
                    case 1://반능동
                        newMissile.boresiteGain = 1.5f;
                        break;
                    case 2://능동
                        newMissile.boresiteGain = 1;
                        break;
                }    
                break;


            case 1://날개
                switch (index)
                {
                    case 0://균형잡힌 날개
                        newMissile.maxgGain = 1.5f;
                        newMissile.dragGain = 1f;
                        break;
                    case 1://소형 날개
                        newMissile.maxgGain = 1;
                        newMissile.dragGain = 0.8f;
                        break;
                    case 2://고양력 날개
                        newMissile.maxgGain = 2.2f;
                        newMissile.dragGain = 1.2f;
                        break;
                }
                break;


            case 2://액추에이터
                switch (index)
                {
                    case 0://단순한 액추에이터
                        newMissile.turnGain = 1f;
                        newMissile.lifeTimeGain = 1f;
                        break;
                    case 1://지속성 액추에이터
                        newMissile.turnGain = 0.75f;
                        newMissile.lifeTimeGain = 1.5f;
                        break;
                    case 2://강력한 액추에이터
                        newMissile.turnGain = 1.5f;
                        newMissile.lifeTimeGain = 0.75f;
                        break;
                }
                break;


            case 3://몸체
                newMissile.MissileBodyTypeSet(index);               
                break;


            case 4://로켓모터
                switch (index)
                {
                    case 0://단거리 로켓
                        newMissile.powerGain = 2.2f;
                        newMissile.fbTimeGain = 0.5f;
                        newMissile.sbTimeGain = 0;
                        break;
                    case 1://부스트-서스테인 로켓
                        newMissile.powerGain = 1.25f;
                        newMissile.fbTimeGain = 0.75f;
                        newMissile.sbTimeGain = 1f;
                        break;
                    case 2://장거리 로켓
                        newMissile.powerGain = 0.9f;
                        newMissile.fbTimeGain = 3f;
                        newMissile.sbTimeGain = 0;
                        break;
                }
                break;


            case 5://특수
                switch (index)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
                break;
        }

        MissileInfoSet();        
    }

    void MissileInfoSet()
    {
        MissileData infoData = newMissile.PrintMissileSpec();

        string seekerInfo;
        string bodyInfo;

        switch(infoData.seekerType)
        {
            case 0:
                seekerInfo = "열추적";
                break;
            case 1:
                seekerInfo = "반능동 레이더 추적";
                break;
            case 2:
                seekerInfo = "능동 레이더 추적";
                break;
            default:
                seekerInfo = "없음";
                break;
        }
        switch(infoData.bodyType)
        {
            case 0:
                bodyInfo = "초소형";
                break;
            case 1:
                bodyInfo = "소형";
                break;
            case 2:
                bodyInfo = "중형";
                break;
            case 3:
                bodyInfo = "대형";
                break;
            case 4:
                bodyInfo = "초대형";
                break;
            default:
                bodyInfo = "선택되지 않음";
                break;
        }

        missileInfoTmp.text = "작동 시간 = " + infoData.lifeTime.ToString() + "초" + "\n" + 
            "연소 시간 1 = " + infoData.firstBurnTime.ToString() + "초" + "\n" +
            "연소 시간 2 = " + infoData.secondBurnTime.ToString() + "초" + "\n" +
            "기동성 = " + (infoData.MAX_G * 0.1f).ToString() + "G" + "\n" +
            "가속력 = " + infoData.enginePower.ToString() + "m/s^2" + "\n" +
            "추적률 = 초당 " + infoData.MAX_TURN_RATE.ToString() + "도" + "\n" +
            "최대 추적각 = " + infoData.MAX_BORESITE.ToString() + "도" + "\n" +
            "항력계수 = " + infoData.defaultDrag.ToString() + "\n" +
            "시커 종류 = " + seekerInfo + "\n" +
            "동체 크기 = " + bodyInfo;
    }

    public void SelectScreenInit()
    {
        foreach(GameObject tool in selectTool)
        {
            tool.gameObject.SetActive(false);
        }
    }

    public class MissileLowData
    {
        public float lifeTime { get; private set; }//표준 = 25;
        public float lifeTimeGain { private get; set; }
        public float firstBurnTime { get; private set; }//표준 = 5;
        public float fbTimeGain { private get; set; }
        public float secondBurnTime { get; private set; }//표준 = 5;
        public float sbTimeGain { private get; set; }
        public float MAX_G { get; private set; }//표준 = 450;
        public float maxgGain { private get; set; }
        public float enginePower { get; private set; }//표준 = 160;
        public float powerGain { private get; set; }
        public float MAX_TURN_RATE { get; private set; }//표준 = 20;
        public float turnGain { private get; set; }
        public float MAX_BORESITE { get; private set; }//표준 = 90;
        public  float boresiteGain { private get; set; }
        public float defaultDrag { get; private set; }//표준 = 0.1f;
        public  float dragGain { private get; set; }
        int seekerType = -1;//열추적 0, 반능동 1, 능동 2;
        public void SeekerSet(int seekertype)
        {
            seekerType = seekertype;
        }
        int bodyType = -1;//초소형 0, 소형 1, 중형 2, 대형 3, 초대형 4

        public void MissileBodyTypeSet(int bodytype)
        {
            bodyType = bodytype;
            switch(bodytype)
            {
                case 0:
                    lifeTime = 10;
                    firstBurnTime = 3;
                    secondBurnTime = 3;
                    MAX_G = 200;
                    enginePower = 200;
                    MAX_TURN_RATE = 20;
                    MAX_BORESITE = 90;
                    defaultDrag = 0.1f;
                    break;
                case 1:
                    lifeTime = 15;
                    firstBurnTime = 5;
                    secondBurnTime = 5;
                    MAX_G = 180;
                    enginePower = 160;
                    MAX_TURN_RATE = 17;
                    MAX_BORESITE = 90;
                    defaultDrag = 0.08f;
                    break;
                case 2:
                    lifeTime = 20;
                    firstBurnTime = 5;
                    secondBurnTime = 5;
                    MAX_G = 160;
                    enginePower = 180;
                    MAX_TURN_RATE = 14;
                    MAX_BORESITE = 90;
                    defaultDrag = 0.07f;
                    break;
                case 3:
                    lifeTime = 25;
                    firstBurnTime = 7;
                    secondBurnTime = 7;
                    MAX_G = 140;
                    enginePower = 130;
                    MAX_TURN_RATE = 12;
                    MAX_BORESITE = 90;
                    defaultDrag = 0.065f;
                    break;
                case 4:
                    lifeTime = 60;
                    firstBurnTime = 10;
                    secondBurnTime = 10;
                    MAX_G = 130;
                    enginePower = 100;
                    MAX_TURN_RATE = 10;
                    MAX_BORESITE = 90;
                    defaultDrag = 0.06f;
                    break;
            }
        }

        public MissileData PrintMissileSpec()//작동시간, 1차 부스터, 2차 부스터, 최대기동, 추력, 추적률, 시커각, 항력, 시커타입(0,1,2), 동체타입(0~4)
        {
            MissileData md = new MissileData();

            md.lifeTime = lifeTime * lifeTimeGain;
            md.firstBurnTime = firstBurnTime * fbTimeGain;
            md.secondBurnTime = secondBurnTime * sbTimeGain;
            md.MAX_G = MAX_G * maxgGain;
            md.enginePower = enginePower * powerGain;
            md.MAX_TURN_RATE = MAX_TURN_RATE * turnGain;
            md.MAX_BORESITE = MAX_BORESITE * boresiteGain;
            md.defaultDrag = defaultDrag * dragGain;
            md.seekerType = seekerType;
            md.bodyType = bodyType;

            return md;
        }
    }

    public MissileData GetMissileSpec()
    {
        MissileData md = newMissile.PrintMissileSpec();

        if (md.lifeTime < 0 || md.firstBurnTime < 0 || md.secondBurnTime < 0 || md.MAX_G < 0 || md.enginePower < 0 ||
            md.MAX_TURN_RATE < 0 || md.MAX_BORESITE < 0 || md.defaultDrag < 0 || md.seekerType < 0 || md.bodyType < 0)
        {
            return null;
        }

        return newMissile.PrintMissileSpec();
    }

    MissileLowData newMissile = new MissileLowData();

    private void Start()
    {
        SelectScreenInit();
    }
}

public class MissileData
{
    public float lifeTime = -1;
    public float firstBurnTime = -1;
    public float secondBurnTime = -1;
    public float MAX_G = -1;
    public float enginePower = -1;
    public float MAX_TURN_RATE = -1;
    public float MAX_BORESITE = -1;
    public float defaultDrag = -1;
    public int seekerType = -1;//열추적 0, 반능동 1, 능동 2;
    public int bodyType = -1;//초소형 0, 소형 1, 중형 2, 대형 3, 초대형 4
}
