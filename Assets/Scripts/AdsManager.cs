using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour {
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
    public InputField gameIDInput;

    public void Init (string id) {
        if (!string.IsNullOrEmpty (id)) gameId = id;
        if (!string.IsNullOrEmpty (gameIDInput.text)) gameId = gameIDInput.text;
        if (!Advertisement.isSupported || Advertisement.isInitialized) {
            Debug.Log ("Could not initialize ads");
            return;
        }

        Debug.Log ("Initializing Unity Ads with game ID: " + gameId);

        UnityIAPManager iapManager = this.GetComponent<UnityIAPManager> ();
        iapManager.Initialize ();
        Advertisement.Initialize (gameId, false);

    }

    private void BannerLoadCallback () {
        Debug.Log ("Unity Ads Log: BannerLoadCallback");
            Advertisement.Banner.Show ();
    }
    private void BannerErrorCallback (string error) {
        Debug.Log ("Unity Ads Log: BannerErrorCallback  " + error);
    }

    public void ShowInterstitial () {
        Debug.Log ("Unity Ads Log: ShowInterstitial");
        Advertisement.Show (interstitialPlacementID);
    }

    public void ShowRewarded () {
        Debug.Log ("Unity Ads Log: ShowRewarded");

        ShowOptions options = new ShowOptions ();
        options.resultCallback = HandleAdShowResult;
        Advertisement.Show (rewardedPlacementID, options);
    }

    public void ShowPromo () {
        Debug.Log ("Unity Ads Log: Promo Shown");

        ShowOptions options = new ShowOptions ();
        options.resultCallback = HandlePromoShowResult;
        Advertisement.Show (iappromoPlacementID, options);
    }

    public void ShowBanner () {
        if (bannerShown) {
            Advertisement.Banner.Hide (true);
        } else {
        BannerLoadOptions options = new BannerLoadOptions ();
        options.loadCallback = BannerLoadCallback;
        options.errorCallback = BannerErrorCallback;
        Advertisement.Banner.Load (bannerPlacementID, options);
        }
    }

    private void HandleAdShowResult (ShowResult result) {
        Debug.LogFormat ("Ad finished with result {0}", result);
    }

    private void HandlePromoShowResult (ShowResult result) {
        Debug.LogFormat ("Promo ad finished with result {0}", result);
    }
}