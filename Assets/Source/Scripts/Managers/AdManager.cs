using GoogleMobileAds.Api;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AdManager : MonoBehaviour, IAdManager
    {
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;

        private const string InterstitialAdID = "ca-app-pub-7721972426864016/7684010590";
        private const string RewardedAdID = "ca-app-pub-3940256099942544/5224354917";
        private const int _maxErrorCount = 6;
        private int _errorCount;
        private bool _isShowAd;

        private void Awake()
        {
            MobileAds.Initialize(initStatus =>
            {
                RequestConfiguration requestConfiguration = new RequestConfiguration
                {
                    TestDeviceIds = new List<string> { "0c7eebe0-fb8d-4054-a508-4ae573cee300", "4729fac9-3274-4976-bfd5-ee5b7ae6c21c", "f529d3ca-ef88-418f-b3cb-5663e42b016c" }
                };
                MobileAds.SetRequestConfiguration(requestConfiguration);


                BuildInterstitialAd();
                BuildRewardedAd();
            });
        }

        public IAdAdapter ShowBetweenScreenAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();

                IAdAdapter adAdapter = new InterstitialAdAdapter(_interstitialAd);
                adAdapter.OnEndShow += BuildInterstitialAd;
                return adAdapter;
            }
            else
            {
                Debug.LogWarning("Interstitial Ad not ready!");
                return null;
            }
        }

        public IAdAdapter ShowRewardedAd(Action<Reward> onRewardEarned)
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((reward) =>
                {
                    Debug.Log("Reward earned: " + reward.Type + " amount: " + reward.Amount);
                    onRewardEarned?.Invoke(reward);
                });
                IAdAdapter adAdapter = new RewardAdAdapter(_rewardedAd);
                adAdapter.OnEndShow += BuildRewardedAd;
                return adAdapter;
            }
            else
            {
                Debug.LogWarning("Rewarded Ad not ready!");
                return new RewardAdAdapter();
            }
        }

        private void BuildInterstitialAd()
        {
            AdRequest request = new AdRequest();
            InterstitialAd.Load(InterstitialAdID, request, HandleInterstitialAdLoaded);
        }

        private void HandleInterstitialAdLoaded(InterstitialAd ad, LoadAdError error)
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Error loading Interstitial Ad: " + error);
                _errorCount++;
                if(_errorCount < _maxErrorCount)
                {
                    Invoke(nameof(BuildInterstitialAd), 5f);
                }
                return;
            }

            _errorCount = 0;
            Debug.Log("Interstitial Ad successfully loaded: " + ad.GetResponseInfo());
            _interstitialAd?.Destroy();
            _interstitialAd = ad;
        }

        private void BuildRewardedAd()
        {
            AdRequest request = new AdRequest();
            RewardedAd.Load(RewardedAdID, request, HandleRewardedAdLoaded);
        }

        private void HandleRewardedAdLoaded(RewardedAd ad, LoadAdError error)
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Error loading Rewarded Ad: " + error);
                _errorCount++;
                if (_errorCount < _maxErrorCount)
                {
                    Invoke(nameof(BuildRewardedAd), 5f);
                }
                return;
            }


            _errorCount = 0;
            Debug.Log("Rewarded Ad loaded: " + ad.GetResponseInfo());
            _rewardedAd?.Destroy();
            _rewardedAd = ad;
        }

        private void ChangeAdStatus(bool isAdRemoved) => _isShowAd = !isAdRemoved;
    }
}
