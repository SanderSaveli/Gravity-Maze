using Cysharp.Threading.Tasks;
using DG.Tweening;
using GoogleMobileAds.Api;
using SanderSaveli.UDK.UI;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorByAdsScreen : ClosableUIScreen
    {
        [SerializeField] private Image _progressImage;
        [SerializeField] private TMP_Text _starText;
        [SerializeField] private string _format = "{0}/{1}";
        [SerializeField] private Button _watchAdsButtpn;
        [SerializeField] private float _fillDuration = 0.5f;
        [SerializeField] private float _fillDelay = 0.5f;

        private IAdsPurchasizeStorage _adsPurchasizeStorage;
        private IAdManager _adManager;
        private IColorManager _colorManager;

        private ColorSheme _currSheme;
        private int _needCount;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IAdsPurchasizeStorage adsPurchasizeStorage, IColorManager colorManager, IAdManager adManager, SignalBus signalBus)
        {
            _adsPurchasizeStorage = adsPurchasizeStorage;
            _adManager = adManager;
            _colorManager = colorManager;
            _signalBus = signalBus;
        }

        public void Init(ColorSheme color, int needCount)
        {
            _currSheme = color;
            _needCount = needCount;
            _progressImage.fillAmount = 0;
            UpdateView();
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _watchAdsButtpn.onClick.AddListener(WatchAd);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _watchAdsButtpn.onClick.RemoveListener(WatchAd);
        }

        private void WatchAd()
        {
            //IAdAdapter adAdapter = _adManager.ShowRewardedAd(IncreaseAds);
            //if (!adAdapter.IsSuccsessShow)
            //{
            //    _signalBus.Fire(new SignalInputOpenMenuPopup(UDK.MenuPopupType.AdError));

            //}
            _signalBus.Fire(new SignalInputOpenMenuPopup(UDK.MenuPopupType.AdError));
        }

        protected void UpdateView()
        {
            int currWatchCount = _adsPurchasizeStorage.GetWatchedAdsPerColor(_currSheme);
            _starText.text = string.Format(_format, currWatchCount, _needCount);
            float fill = (float)currWatchCount / (float)_needCount;

            Sequence sequence = DOTween.Sequence();
            sequence
                .AppendInterval(_fillDelay)
                .Append(_progressImage.DOFillAmount(fill, _fillDuration).SetLink(_progressImage.gameObject));
        }

        private async void IncreaseAds(Reward reward)
        {
            _adsPurchasizeStorage.AddWatch(_currSheme);
            UpdateView();

            if (_adsPurchasizeStorage.GetWatchedAdsPerColor(_currSheme) >= _needCount)
            {
                await UniTask.WaitForSeconds(_fillDelay + _fillDuration);
                _colorManager.ActiveSheme.Value = _currSheme;
                HandleBack();
            }
        }
    }
}
