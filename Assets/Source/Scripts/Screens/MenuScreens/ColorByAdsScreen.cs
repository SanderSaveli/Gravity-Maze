using DG.Tweening;
using GoogleMobileAds.Api;
using SanderSaveli.UDK.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorByAdsScreen : ClosableUIScreen
    {
        [Header("Components")]
        [SerializeField] private Image _progressImage;
        [SerializeField] private Button _previewButton;
        [SerializeField] private Image _previewImage;
        [SerializeField] private TMP_Text _starText;
        [SerializeField] private Button _watchAdsButtpn;
        [SerializeField] private UnlockColorScreen _unlockScreen;
        [SerializeField] private WaveSpawner _waveSpawner;

        [Header("Params")]
        [SerializeField] private string _format = "{0}/{1}";
        [SerializeField] private float _fillDuration = 0.5f;
        [SerializeField] private float _fillDelay = 0.5f;


        private IAdsPurchasizeStorage _adsPurchasizeStorage;
        private IAdManager _adManager;
        private IColorManager _colorManager;
        private IAnalyticManager _analyticManager;

        private int _needCount;
        private ColorSheme _colorSheme;
        private bool _isPreviewActive;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IAdsPurchasizeStorage adsPurchasizeStorage, IColorManager colorManager, IAdManager adManager, SignalBus signalBus, IAnalyticManager analyticManager)
        {
            _adsPurchasizeStorage = adsPurchasizeStorage;
            _adManager = adManager;
            _colorManager = colorManager;
            _signalBus = signalBus;
            _analyticManager = analyticManager;
        }

        public void Init(ColorContext colorContext)
        {
            _colorSheme = colorContext.ColorSheme;
            _needCount = colorContext.AdsToUnlock;
            _progressImage.fillAmount = 0;
            _previewImage.color = _colorManager.GetActiveColorOfSheme(_colorSheme);
            _waveSpawner.SetWaveColor(_colorManager.GetActiveColorOfSheme(_colorSheme));
            UpdateView();
        }
        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _previewButton.onClick.AddListener(HandlePreview);
            _watchAdsButtpn.onClick.AddListener(WatchAd);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _previewButton.onClick.RemoveListener(HandlePreview);
            _watchAdsButtpn.onClick.RemoveListener(WatchAd);
        }
        public override void Hide(Action callback = null)
        {
            if (_isPreviewActive)
            {
                _colorManager.ShowActiveSheme();
                _isPreviewActive = false;
            }
            base.Hide(callback);
        }

        private void HandlePreview()
        {
            if (!_isPreviewActive)
            {
                Debug.Log("Preview: " + _colorSheme);
                _colorManager.PreviewSheme(_colorSheme);
                _isPreviewActive = true;
            }
            else
            {
                _colorManager.ShowActiveSheme();
                _isPreviewActive = false;
            }
        }

        private void WatchAd()
        {
            IncreaseAds(new Reward());
            return;
            IAdAdapter adAdapter = _adManager.ShowRewardedAd(IncreaseAds);
            if (!adAdapter.IsSuccsessShow)
            {
                _signalBus.Fire(new SignalInputOpenMenuPopup(UDK.MenuPopupType.AdError));
            }
            else
            {
                _analyticManager.SendAdWatchedEvent(_colorSheme);
            }
        }

        protected void UpdateView()
        {
            int currWatchCount = _adsPurchasizeStorage.GetWatchedAdsPerColor(_colorSheme);
            _starText.text = string.Format(_format, currWatchCount, _needCount);
            float fill = (float)currWatchCount / (float)_needCount;

            Sequence sequence = DOTween.Sequence();
            sequence
                .AppendInterval(_fillDelay)
                .Append(_progressImage.DOFillAmount(fill, _fillDuration).SetLink(_progressImage.gameObject));
        }

        private void IncreaseAds(Reward reward)
        {
            _adsPurchasizeStorage.AddWatch(_colorSheme);
            UpdateView();

            if (_adsPurchasizeStorage.GetWatchedAdsPerColor(_colorSheme) >= _needCount)
            {
                ColorUnlocked();
            }
        }

        private void ColorUnlocked()
        {
            _analyticManager.SendUnlockColorForAdEvent(_colorSheme);
            _unlockScreen.Init(_colorSheme);
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.OpenColor));
        }
    }
}
