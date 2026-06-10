using System;
using AppodealStack.Monetization.Api;
using AppodealStack.Monetization.Common;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class AppodealAdManager : MonoBehaviour, IAdManager
    {
        [Header("App Keys")]
        [SerializeField] private string _androidAppKey;
        [SerializeField] private string _iosAppKey;

        [Header("Parameters")]
        [SerializeField] private bool _testMode = true;
        [SerializeField] private AppodealLogLevel _logLevel = AppodealLogLevel.None;
        [SerializeField] private string _placement = "default";
        [SerializeField] private bool _autoCache = true;

        private bool _isInitialized;
        private bool _isShowing;
        private int _showingAdType;
        private Action<AdReward> _pendingRewardCallback;
        private AppodealAdAdapter _currentAdapter;

        private string AppKey
        {
            get
            {
#if UNITY_IOS
                return _iosAppKey;
#elif UNITY_ANDROID
                return _androidAppKey;
#else
                return _androidAppKey;
#endif
            }
        }

        private void Awake()
        {
            SubscribeToCallbacks();

            if (string.IsNullOrWhiteSpace(AppKey))
            {
                Debug.LogWarning("Appodeal app key is not set.");
                return;
            }

            Appodeal.SetLogLevel(_logLevel);
            Appodeal.SetTesting(_testMode);
            Appodeal.SetAutoCache(AppodealAdType.Interstitial, _autoCache);
            Appodeal.SetAutoCache(AppodealAdType.RewardedVideo, _autoCache);

            int adTypes = AppodealAdType.Interstitial | AppodealAdType.RewardedVideo;
            Appodeal.Initialize(AppKey, adTypes);
        }

        private void OnDestroy()
        {
            UnsubscribeFromCallbacks();
        }

        public IAdAdapter ShowBetweenScreenAd()
        {
            if (CanShow(AppodealAdType.Interstitial) && !Appodeal.IsPrecache(AppodealAdType.Interstitial))
            {
                return ShowAd(AppodealAdType.Interstitial, AppodealShowStyle.Interstitial);
            }

            Debug.LogWarning("Appodeal interstitial ad is not ready!");
            CacheAd(AppodealAdType.Interstitial);
            return null;
        }

        public IAdAdapter ShowRewardedAd(Action<AdReward> onRewardEarned)
        {
            if (CanShow(AppodealAdType.RewardedVideo))
            {
                _pendingRewardCallback = onRewardEarned;
                return ShowAd(AppodealAdType.RewardedVideo, AppodealShowStyle.RewardedVideo);
            }

            Debug.LogWarning("Appodeal rewarded ad is not ready!");
            CacheAd(AppodealAdType.RewardedVideo);
            return new AppodealAdAdapter(false);
        }

        private bool CanShow(int adType)
        {
            return _isInitialized &&
                   !_isShowing &&
                   Appodeal.IsLoaded(adType) &&
                   Appodeal.CanShow(adType, _placement);
        }

        private IAdAdapter ShowAd(int adType, int showStyle)
        {
            _isShowing = true;
            _showingAdType = adType;
            _currentAdapter = new AppodealAdAdapter(true);

            if (Appodeal.Show(showStyle, _placement))
            {
                return _currentAdapter;
            }

            CompleteShow(adType);
            return new AppodealAdAdapter(false);
        }

        private void CompleteShow(int adType)
        {
            if (adType != _showingAdType)
            {
                return;
            }

            _isShowing = false;
            _showingAdType = 0;
            _pendingRewardCallback = null;
            _currentAdapter?.Complete();
            _currentAdapter = null;
            CacheAd(adType);
        }

        private void CacheAd(int adType)
        {
            if (!_autoCache && Appodeal.IsInitialized(adType))
            {
                Appodeal.Cache(adType);
            }
        }

        private void SubscribeToCallbacks()
        {
            AppodealCallbacks.Sdk.OnInitialized += OnAppodealInitialized;

            AppodealCallbacks.Interstitial.OnLoaded += OnInterstitialLoaded;
            AppodealCallbacks.Interstitial.OnFailedToLoad += OnInterstitialFailedToLoad;
            AppodealCallbacks.Interstitial.OnShowFailed += OnInterstitialShowFailed;
            AppodealCallbacks.Interstitial.OnClosed += OnInterstitialClosed;
            AppodealCallbacks.Interstitial.OnExpired += OnInterstitialExpired;

            AppodealCallbacks.RewardedVideo.OnLoaded += OnRewardedVideoLoaded;
            AppodealCallbacks.RewardedVideo.OnFailedToLoad += OnRewardedVideoFailedToLoad;
            AppodealCallbacks.RewardedVideo.OnShowFailed += OnRewardedVideoShowFailed;
            AppodealCallbacks.RewardedVideo.OnClosed += OnRewardedVideoClosed;
            AppodealCallbacks.RewardedVideo.OnFinished += OnRewardedVideoFinished;
            AppodealCallbacks.RewardedVideo.OnExpired += OnRewardedVideoExpired;
        }

        private void UnsubscribeFromCallbacks()
        {
            AppodealCallbacks.Sdk.OnInitialized -= OnAppodealInitialized;

            AppodealCallbacks.Interstitial.OnLoaded -= OnInterstitialLoaded;
            AppodealCallbacks.Interstitial.OnFailedToLoad -= OnInterstitialFailedToLoad;
            AppodealCallbacks.Interstitial.OnShowFailed -= OnInterstitialShowFailed;
            AppodealCallbacks.Interstitial.OnClosed -= OnInterstitialClosed;
            AppodealCallbacks.Interstitial.OnExpired -= OnInterstitialExpired;

            AppodealCallbacks.RewardedVideo.OnLoaded -= OnRewardedVideoLoaded;
            AppodealCallbacks.RewardedVideo.OnFailedToLoad -= OnRewardedVideoFailedToLoad;
            AppodealCallbacks.RewardedVideo.OnShowFailed -= OnRewardedVideoShowFailed;
            AppodealCallbacks.RewardedVideo.OnClosed -= OnRewardedVideoClosed;
            AppodealCallbacks.RewardedVideo.OnFinished -= OnRewardedVideoFinished;
            AppodealCallbacks.RewardedVideo.OnExpired -= OnRewardedVideoExpired;
        }

        private void OnAppodealInitialized(object sender, SdkInitializedEventArgs args)
        {
            _isInitialized = true;

            if (args.Errors != null && args.Errors.Count > 0)
            {
                Debug.LogError($"Appodeal initialized with errors: {string.Join(", ", args.Errors)}");
            }

            CacheAd(AppodealAdType.Interstitial);
            CacheAd(AppodealAdType.RewardedVideo);
        }

        private void OnInterstitialLoaded(object sender, AdLoadedEventArgs args)
        {
            Debug.Log($"Appodeal interstitial ad loaded. Is precache: {args.IsPrecache}");
        }

        private void OnInterstitialFailedToLoad(object sender, EventArgs args)
        {
            Debug.LogWarning("Appodeal interstitial ad failed to load.");
            CacheAd(AppodealAdType.Interstitial);
        }

        private void OnInterstitialShowFailed(object sender, EventArgs args)
        {
            Debug.LogWarning("Appodeal interstitial ad failed to show.");
            CompleteShow(AppodealAdType.Interstitial);
        }

        private void OnInterstitialClosed(object sender, EventArgs args)
        {
            CompleteShow(AppodealAdType.Interstitial);
        }

        private void OnInterstitialExpired(object sender, EventArgs args)
        {
            Debug.Log("Appodeal interstitial ad expired.");
            CacheAd(AppodealAdType.Interstitial);
        }

        private void OnRewardedVideoLoaded(object sender, AdLoadedEventArgs args)
        {
            Debug.Log($"Appodeal rewarded ad loaded. Is precache: {args.IsPrecache}");
        }

        private void OnRewardedVideoFailedToLoad(object sender, EventArgs args)
        {
            Debug.LogWarning("Appodeal rewarded ad failed to load.");
            CacheAd(AppodealAdType.RewardedVideo);
        }

        private void OnRewardedVideoShowFailed(object sender, EventArgs args)
        {
            Debug.LogWarning("Appodeal rewarded ad failed to show.");
            CompleteShow(AppodealAdType.RewardedVideo);
        }

        private void OnRewardedVideoClosed(object sender, RewardedVideoClosedEventArgs args)
        {
            CompleteShow(AppodealAdType.RewardedVideo);
        }

        private void OnRewardedVideoFinished(object sender, RewardedVideoFinishedEventArgs args)
        {
            AdReward reward = new AdReward(args.Currency, args.Amount);
            Debug.Log($"Appodeal reward earned: {reward.Type} amount: {reward.Amount}");
            _pendingRewardCallback?.Invoke(reward);
            _pendingRewardCallback = null;
        }

        private void OnRewardedVideoExpired(object sender, EventArgs args)
        {
            Debug.Log("Appodeal rewarded ad expired.");
            CacheAd(AppodealAdType.RewardedVideo);
        }
    }
}
