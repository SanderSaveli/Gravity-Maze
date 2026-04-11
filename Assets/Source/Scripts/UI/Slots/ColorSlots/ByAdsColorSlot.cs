using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ByAdsColorSlot : ClosableColorSlot
    {
        [Header("Components")]
        [SerializeField] private ColorByAdsScreen _colorByStarScreen;

        [Header("Params")]
        [SerializeField] private int _needAdsToUnlock;

        private IAdsPurchasizeStorage _adsPurchasizeStorage;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IAdsPurchasizeStorage adsPurchasizeStorage, SignalBus signalBus)
        {
            _adsPurchasizeStorage = adsPurchasizeStorage;
            _signalBus = signalBus;
        }

        protected override void OpenPreview()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.ColorByAds));
            _colorByStarScreen.Init(Value, _needAdsToUnlock);
        }

        protected override bool IsOpened() => _adsPurchasizeStorage.GetWatchedAdsPerColor(Value) >= _needAdsToUnlock;
    }
}
