#if UNITY_ANDROID || UNITY_IOS
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
//using GS.AA;
using System.Collections;

public class AdmobAds : MonoBehaviour
{
    string GameID = "ca-app-pub-4631286883932087~6054133570";   //Pong Soccer

    // Dummy ads
    string bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    string InterstitialAdID = "ca-app-pub-3940256099942544/1033173712";
    string rewarded_Ad_ID = "ca-app-pub-3940256099942544/5224354917";

    // Real ads
    string realBannerAdId = "ca-app-pub-4631286883932087/2055448317";
    // string realInterstitialAdID = "ca-app-pub-4631286883932087/1150225752"; [NOT SET YET]
    // string realRewarded_Ad_ID = "ca-app-pub-4631286883932087/3034153064"; [NOT SET YET]

    public BannerView bannerAd;
    public InterstitialAd interstitial;
    public RewardedAd rewardedAd;

    public static AdmobAds instance;

    public Action OnReward;
    public Action OnInterestialAdsComplete;
    public Action OnRewardFailed;

    private Action OnRewardAdComplete;
    private Action OnRewardAdCanceled;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);


    }

    // Start is called before the first frame update
    void Start()
    {
        bannerAdId = realBannerAdId;
        // InterstitialAdID = realInterstitialAdID;
        // rewarded_Ad_ID = realRewarded_Ad_ID;

        // MobileAds.Initialize(GameID);
        MobileAds.Initialize(initStatus =>
        {
            //requestInterstital();
            // loadRewardVideo();    [Note: Uncomment it when gonna use Reward Ads]
        });

    }

    #region rewarded Video Ads

    public void loadRewardVideo()
    {
        rewardedAd = new RewardedAd(rewarded_Ad_ID);

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        //rewardedAd.OnAdLeavingApplication += HandleOnRewardAdleavingApp;

    }

    /// rewarded video events //////////////////////////////////////////////

    public event EventHandler<EventArgs> OnAdLoaded;

    public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

    public event EventHandler<EventArgs> OnAdOpening;

    public event EventHandler<EventArgs> OnAdStarted;

    public event EventHandler<EventArgs> OnAdClosed;

    public event EventHandler<Reward> OnAdRewarded;

    public event EventHandler<EventArgs> OnAdLeavingApplication;

    public event EventHandler<EventArgs> OnAdCompleted;

    /// Rewared events //////////////////////////



    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //Debug.Log("Video Loaded");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // Debug.Log("Video not loaded");
        OnRewardAdCanceled?.Invoke();
        // OnRewardFailed?.Invoke();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        // Debug.Log("Video Loading");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        // Debug.Log("Video Loading failed");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        // Debug.Log("Video Loading failed");
        //OnRewardFailed?.Invoke();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        StartCoroutine(Delay());
        //GameManager.Instance.SkipLevel();
    }

    public void HandleOnRewardAdleavingApp(object sender, EventArgs args)
    {
        //  Debug.Log("when user clicks the video and open a new window");
    }

    public bool IsRewarededVideoLoaded()
    {
        return rewardedAd.IsLoaded();
    }

    public void ShowRewardedAd(Action onRewardAdShown, Action onRewardAdCanceled)
    {
        if (!IsRewarededVideoLoaded())
        {
            loadRewardVideo();
        }
        else
        {
            OnRewardAdComplete = onRewardAdShown;
            OnRewardAdCanceled = onRewardAdCanceled;
            rewardedAd.Show();
        }
    }

    public void showVideoAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded Video ad not loaded");
            loadRewardVideo();
        }
    }

    #endregion

    #region banner

    public void reqBannerAd(AdPosition adPosition = AdPosition.Bottom)
    {
        this.bannerAd = new BannerView(bannerAdId, AdSize.Banner, adPosition);

        // Called when an ad request has successfully loaded.
        this.bannerAd.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerAd.LoadAd(request);

    }


    public void hideBanner()
    {
        this.bannerAd.Hide();
    }

    #endregion

    #region interstitial

    public void requestInterstital()
    {
        this.interstitial = new InterstitialAd(InterstitialAdID);

        this.interstitial.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.interstitial.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.interstitial.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.interstitial.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);
    }

    public void ShowInterstitialAd()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }

        else
        {
            requestInterstital();
        }
    }

    public bool IsInterestitialLoaded()
    {
        return interstitial.IsLoaded();
    }

    #endregion

    #region adDelegates

    //Delegates that i dont know
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        // Debug.Log("Ad Loaded");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // Debug.Log("couldnt load ad" + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        Time.timeScale = 0f;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //Debug.Log("Ad Closed");
        Time.timeScale = 1f;
        requestInterstital(); // Optional : in case you want to load another interstial ad rightaway
        OnInterestialAdsComplete?.Invoke();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion

    IEnumerator Delay()
    {
        yield return new WaitForEndOfFrame();
        Time.timeScale = 1f;
        //GameManager.Instance.SkipLevel();
        OnRewardAdComplete?.Invoke();

        AdmobAds.instance.loadRewardVideo();
    }

}
#endif