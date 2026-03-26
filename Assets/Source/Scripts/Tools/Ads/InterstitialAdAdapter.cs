using GoogleMobileAds.Api;
using System;

namespace SanderSaveli.GravityMaze
{
    public class InterstitialAdAdapter : IAdAdapter
    {
        public Action OnEndShow { get; set; }

        public InterstitialAdAdapter(InterstitialAd interstitialAd)
        {
            interstitialAd.OnAdFullScreenContentClosed += () => OnEndShow?.Invoke();
            interstitialAd.OnAdFullScreenContentFailed += (_) => OnEndShow?.Invoke();
        }
    }
}
