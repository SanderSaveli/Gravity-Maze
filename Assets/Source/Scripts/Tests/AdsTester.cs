using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AdsTester : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Button _rewardButton;
        private IAdManager _adManager;
        private int couter;

        [Inject]
        public void Construct(IAdManager adManager)
        {
            _adManager = adManager;
        }

        private void OnEnable()
        {
            _text.text = couter.ToString();
            _button.onClick.AddListener(ShowAd);
            _rewardButton.onClick.AddListener(ShowReward);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ShowAd);
            _rewardButton.onClick.AddListener(ShowReward);
        }

        private void ShowAd()
        {
            _adManager.ShowBetweenScreenAd();
        }

        private void ShowReward()
        {
            _adManager.ShowRewardedAd(HandleReward);
        }

        private void HandleReward(Reward reward)
        {
            Debug.Log($"GetReward  with type {reward.Type} and amount: {reward.Amount}");
            couter++;
            _text.text = couter.ToString();
        }
    }
}
