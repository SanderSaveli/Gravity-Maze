using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ByAdsColorSlot : ClosableColorSlot
    {
        private IAdsPurchasizeStorage _adsPurchasizeStorage;

        [Inject]
        public void Construct(IAdsPurchasizeStorage adsPurchasizeStorage)
        {
            _adsPurchasizeStorage = adsPurchasizeStorage;
        }

        protected override bool IsOpened() =>
            _adsPurchasizeStorage.GetWatchedAdsPerColor(Value) >= ColorContext.AdsToUnlock;
    }
}
