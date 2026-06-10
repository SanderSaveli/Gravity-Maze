using GoogleMobileAds.Api;
using System;

namespace SanderSaveli.GravityMaze
{
    public class RewardAdAdapter : IAdAdapter
    {
        public Action OnEndShow { get; set; }

        public bool IsSuccsessShow { get; private set; }

        public RewardAdAdapter(RewardedAd ad)
        {
            IsSuccsessShow = true;
            ad.OnAdFullScreenContentClosed += () => OnEndShow?.Invoke();
            ad.OnAdFullScreenContentFailed += (_) => OnEndShow?.Invoke();
        }

        public RewardAdAdapter()
        {
            IsSuccsessShow = false;
        }
    }
}
