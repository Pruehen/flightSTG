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

        // ����� ���� SDK�� �ʱ�ȭ��.
        MobileAds.Initialize(initStatus => { });

        //���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.LoadAd(request);


        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args) { MonoBehaviour.print("HandleRewardedAdLoaded event received"); }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)// ���� �ε尡 �������� �� ȣ��
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
    public void HandleUserEarnedReward(object sender, Reward args)// ���� ��û�� �� ������ �޾ƾ� �� �� ȣ��
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
    public void ShowAds()//���� �����ϴ� �Լ�. �ܺο��� ȣ��
    {
        if (this.rewardedAd.CanShowAd())
        {
            this.rewardedAd.Show();
        }
    }
}
