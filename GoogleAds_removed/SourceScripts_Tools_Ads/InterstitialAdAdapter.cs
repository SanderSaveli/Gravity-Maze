using GoogleMobileAds.Api;
using System;

namespace SanderSaveli.GravityMaze
{
    public class InterstitialAdAdapter : IAdAdapter
    {
        public Action OnEndShow { get; set; }

        public bool IsSuccsessShow { get; private set; }

        public InterstitialAdAdapter(InterstitialAd interstitialAd)
        {
            IsSuccsessShow = true;

            interstitialAd.OnAdFullScreenContentClosed += () => OnEndShow?.Invoke();
            interstitialAd.OnAdFullScreenContentFailed += (_) => OnEndShow?.Invoke();
        }

        public InterstitialAdAdapter()
        {
            IsSuccsessShow = false;
        }
    }
}
