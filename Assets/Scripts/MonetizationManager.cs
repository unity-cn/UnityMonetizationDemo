using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.UI;

public class MonetizationManager : MonoBehaviour {
#if UNITY_IOS
    private string gameId = "1742078"; // Your iOS game ID here
#elif UNITY_ANDROID
    private string gameId = "1742077"; // Your Android game ID here
#else
    private string gameId = "0123456"; // Prevents Editor Errors
#endif

    private string interstitialPlacementID = "video";
    private string rewardedPlacementID = "rewardedVideo";
    private string iappromoPlacementID = "testIAPPromo";
    private string bannerPlacementID = "banner";
    private bool bannerShown = false;
    public bool testMode;

    public InputField gameIDInput;
    public void Init (string id) {
        if (!string.IsNullOrEmpty (id)) gameId = id;
        if (!string.IsNullOrEmpty (gameIDInput.text)) gameId = gameIDInput.text;
        if (!Monetization.isSupported || Monetization.isInitialized) {
            Debug.Log ("Could not initialize ads");
            return;
        }

        Debug.Log ("Initializing Unity Ads with game ID: " + gameId);

        UnityIAPManager iapManager = this.GetComponent<UnityIAPManager> ();
        iapManager.Initialize ();

        Monetization.Initialize (gameId, testMode);
    }

    private void OnEnable () {
        Monetization.onPlacementContentReady += PlacementContentReady;
    }

    private void OnDisable () {
        Monetization.onPlacementContentReady -= PlacementContentReady;
    }

    void PlacementContentReady (object sender, PlacementContentReadyEventArgs e) {
        Debug.Log ("Unity Monetization Log: PlacementContentReady  Placement = " + e.placementId);
    }

    public bool IsReady (string placement) {
        PlacementContent placementContent = Monetization.GetPlacementContent (placement);
        if (placementContent == null) return false;
        return placementContent.ready;
    }
    private void BannerLoadCallback () {
        Debug.Log ("Unity Monetization Log: BannerLoadCallback");
        UnityEngine.Advertisements.Advertisement.Banner.Show ();
    }
    private void BannerErrorCallback (string error) {
        Debug.Log ("Unity Monetization Log: BannerErrorCallback  " + error);
    }

    public void ShowInterstitial () {
        Debug.Log ("Unity Monetization Log: ShowInterstitial");
        ShowAdPlacementContent placementContent = (ShowAdPlacementContent) Monetization.GetPlacementContent (interstitialPlacementID);
        if (placementContent == null) return;

        placementContent.Show (HandleAdShowResult);
    }

    public void ShowRewarded () {
        Debug.Log ("Unity Monetization Log: ShowRewarded");

        ShowAdPlacementContent placementContent = (ShowAdPlacementContent) Monetization.GetPlacementContent (rewardedPlacementID);
        if (placementContent == null) return;

        placementContent.Show (HandleAdShowResult);
    }

    public void ShowPromo () {
        Debug.Log ("Unity Monetization Log: Promo Shown");

        ShowAdPlacementContent placementContent = (PromoAdPlacementContent) Monetization.GetPlacementContent (iappromoPlacementID);
        if (placementContent == null) return;

        placementContent.Show (HandlePromoShowResult);
    }

    public void ShowBanner () {
        if (bannerShown) {
            UnityEngine.Advertisements.Advertisement.Banner.Hide (true);
        } else {
            UnityEngine.Advertisements.BannerLoadOptions options = new UnityEngine.Advertisements.BannerLoadOptions ();
            options.loadCallback = BannerLoadCallback;
            options.errorCallback = BannerErrorCallback;
            UnityEngine.Advertisements.Advertisement.Banner.Load (bannerPlacementID, options);
        }
    }

    private void HandleAdShowResult (ShowResult result) {
        Debug.LogFormat ("Unity Monetization Log: Ad finished with result {0}", result);
    }

    private void HandlePromoShowResult (ShowResult result) {
        Debug.LogFormat ("Unity Monetization Log: Promo ad finished with result {0}", result);
    }

}