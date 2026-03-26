using GoogleMobileAds.Api;
using System;

namespace SanderSaveli.GravityMaze
{
    public class RewardAdAdapter : IAdAdapter
    {
        public Action OnEndShow { get; set; }

        public RewardAdAdapter(RewardedAd ad)
        {
            ad.OnAdFullScreenContentClosed += () => OnEndShow?.Invoke();
            ad.OnAdFullScreenContentFailed += (_) => OnEndShow?.Invoke();
        }
    }
}
