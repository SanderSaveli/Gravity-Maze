using GoogleMobileAds.Api;
using System;

namespace SanderSaveli.GravityMaze
{
    public interface IAdManager
    {
        public IAdAdapter ShowBetweenScreenAd();
        public IAdAdapter ShowRewardedAd(Action<Reward> onRewardEarned);
    }
}
