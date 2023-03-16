using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoogleAds : MonoBehaviour
{
    private RewardedAd rewardedAd;

    [SerializeField]
    string adUnitId;

    [Obsolete]
    public void Start()
    {
        //Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
        //PlayScene_Manager = GameObject.FindObjectOfType<PlayScene_Manager>();

        adUnitId = "ca-app-pub-5969578542325675/4612190471";

        // 모바일 광고 SDK를 초기화함.
        MobileAds.Initialize(initStatus => { });

        //광고 로드 : RewardedAd 객체의 loadAd메서드에 AdRequest 인스턴스를 넣음
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);


        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // 광고 로드가 완료되면 호출
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // 광고 로드가 실패했을 때 호출
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // 광고가 표시될 때 호출(기기 화면을 덮음)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // 광고 표시가 실패했을 때 호출
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// 광고를 시청한 후 보상을 받아야할 때 호출
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // 닫기 버튼을 누르거나 뒤로가기 버튼을 눌러 동영상 광고를 닫을 때 호출
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) { MonoBehaviour.print("HandleRewardedAdLoaded event received"); }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)// 광고 로드가 실패했을 때 호출
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) { MonoBehaviour.print("HandleRewardedAdOpening event received"); }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) { this.CreateAndLoadRewardedAd(); }

    [Obsolete]
    public void CreateAndLoadRewardedAd()
    {
        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public MissionSceneManager rewardObject;
    public void HandleUserEarnedReward(object sender, Reward args)// 광고를 시청한 후 보상을 받아야 할 때 호출
    {
        //if (Quiz_Manager == null) Quiz_Manager = GameObject.FindObjectOfType<Quiz_Manager>();
        //Quiz_Manager.PostADs();
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type + adUnitId);

        if(rewardObject != null)
        {
            rewardObject.PostADs();
        }
    }

    [Obsolete]
    public void ShowAds()//광고를 시작하는 함수. 외부에서 호출
    {
        if (this.rewardedAd.CanShowAd())
        {
            this.rewardedAd.Show();
        }
    }
}
