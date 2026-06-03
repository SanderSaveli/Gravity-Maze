using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace SanderSaveli.GravityMaze
{
    public class AdManager : MonoBehaviour, IAdManager, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [Header("Game Ids")]
        [SerializeField] private string _androidGameId;
        [SerializeField] private string _iosGameId;
        [SerializeField] private bool _testMode = true;

        [Header("Ad Units")]
        [SerializeField] private string _androidInterstitialAdUnitId = "Interstitial_Android";
        [SerializeField] private string _iosInterstitialAdUnitId = "Interstitial_iOS";
        [SerializeField] private string _androidRewardedAdUnitId = "Rewarded_Android";
        [SerializeField] private string _iosRewardedAdUnitId = "Rewarded_iOS";

        [Header("Retry")]
        [SerializeField] private int _maxLoadErrorCount = 6;
        [SerializeField] private float _reloadDelay = 5f;

        private int _interstitialLoadErrorCount;
        private int _rewardedLoadErrorCount;
        private bool _isInitialized;
        private bool _isInterstitialLoaded;
        private bool _isRewardedLoaded;
        private bool _isShowing;
        private string _showingAdUnitId;
        private Action<AdReward> _pendingRewardCallback;
        private UnityAdAdapter _currentAdapter;

        private string GameId
        {
            get
            {
#if UNITY_IOS
                return _iosGameId;
#elif UNITY_ANDROID
                return _androidGameId;
#else
                return _androidGameId;
#endif
            }
        }

        private string InterstitialAdUnitId
        {
            get
            {
#if UNITY_IOS
                return _iosInterstitialAdUnitId;
#elif UNITY_ANDROID
                return _androidInterstitialAdUnitId;
#else
                return _androidInterstitialAdUnitId;
#endif
            }
        }

        private string RewardedAdUnitId
        {
            get
            {
#if UNITY_IOS
                return _iosRewardedAdUnitId;
#elif UNITY_ANDROID
                return _androidRewardedAdUnitId;
#else
                return _androidRewardedAdUnitId;
#endif
            }
        }

        private void Awake()
        {
            if (Advertisement.isInitialized)
            {
                OnInitializationComplete();
                return;
            }

            if (string.IsNullOrWhiteSpace(GameId))
            {
                Debug.LogWarning("Unity Ads game id is not set.");
                return;
            }

            Advertisement.Initialize(GameId, _testMode, this);
        }

        public IAdAdapter ShowBetweenScreenAd()
        {
            if (_isInitialized && _isInterstitialLoaded && !_isShowing)
            {
                _isInterstitialLoaded = false;
                return ShowAd(InterstitialAdUnitId);
            }

            Debug.LogWarning("Unity interstitial ad is not ready!");
            return null;
        }

        public IAdAdapter ShowRewardedAd(Action<AdReward> onRewardEarned)
        {
            if (_isInitialized && _isRewardedLoaded && !_isShowing)
            {
                _pendingRewardCallback = onRewardEarned;
                _isRewardedLoaded = false;
                return ShowAd(RewardedAdUnitId);
            }

            Debug.LogWarning("Unity rewarded ad is not ready!");
            return new UnityAdAdapter(false);
        }

        public void OnInitializationComplete()
        {
            _isInitialized = true;
            LoadInterstitialAd();
            LoadRewardedAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Unity Ads initialization failed: {error} - {message}");
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            if (adUnitId == InterstitialAdUnitId)
            {
                _interstitialLoadErrorCount = 0;
                _isInterstitialLoaded = true;
                Debug.Log("Unity interstitial ad loaded.");
                return;
            }

            if (adUnitId == RewardedAdUnitId)
            {
                _rewardedLoadErrorCount = 0;
                _isRewardedLoaded = true;
                Debug.Log("Unity rewarded ad loaded.");
            }
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Unity ad failed to load {adUnitId}: {error} - {message}");

            if (adUnitId == InterstitialAdUnitId)
            {
                RetryLoad(ref _interstitialLoadErrorCount, LoadInterstitialAd);
                return;
            }

            if (adUnitId == RewardedAdUnitId)
            {
                RetryLoad(ref _rewardedLoadErrorCount, LoadRewardedAd);
            }
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Unity ad failed to show {adUnitId}: {error} - {message}");
            CompleteShow(adUnitId);
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            Debug.Log($"Unity ad show started: {adUnitId}");
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
            Debug.Log($"Unity ad clicked: {adUnitId}");
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId == RewardedAdUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                AdReward reward = new AdReward();
                Debug.Log($"Unity reward earned: {reward.Type} amount: {reward.Amount}");
                _pendingRewardCallback?.Invoke(reward);
            }

            CompleteShow(adUnitId);
        }

        private IAdAdapter ShowAd(string adUnitId)
        {
            _isShowing = true;
            _showingAdUnitId = adUnitId;
            _currentAdapter = new UnityAdAdapter(true);
            Advertisement.Show(adUnitId, this);
            return _currentAdapter;
        }

        private void CompleteShow(string adUnitId)
        {
            if (adUnitId != _showingAdUnitId)
            {
                return;
            }

            _isShowing = false;
            _showingAdUnitId = null;
            _pendingRewardCallback = null;
            _currentAdapter?.Complete();
            _currentAdapter = null;

            if (adUnitId == InterstitialAdUnitId)
            {
                LoadInterstitialAd();
            }
            else if (adUnitId == RewardedAdUnitId)
            {
                LoadRewardedAd();
            }
        }

        private void LoadInterstitialAd()
        {
            Advertisement.Load(InterstitialAdUnitId, this);
        }

        private void LoadRewardedAd()
        {
            Advertisement.Load(RewardedAdUnitId, this);
        }

        private void RetryLoad(ref int errorCount, Action loadAction)
        {
            errorCount++;
            if (errorCount < _maxLoadErrorCount)
            {
                Invoke(loadAction.Method.Name, _reloadDelay);
            }
        }
    }
}
