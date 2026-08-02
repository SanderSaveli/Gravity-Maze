using UnityEngine;
using GameAnalyticsSDK;

namespace SanderSaveli.GravityMaze
{
    public class GameAnalyticsManager : MonoBehaviour, IAnalyticManager
    {
        private bool _initialized;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;

            GameAnalytics.Initialize();
            _initialized = true;

            Debug.Log("GameAnalytics initialized");
        }

        public void SendLevelStartEvent(int levelNumber)
        {
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Start,
                "level",
                levelNumber.ToString()
            );
        }

        public void SendLevelCompleteEvent(int levelNumber, float seconds, ColorSheme activeSheme, int starCount)
        {
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Complete,
                "level",
                levelNumber.ToString(),
                starCount
            );

            GameAnalytics.NewDesignEvent(
                $"level_time:{levelNumber}",
                seconds
            );

            GameAnalytics.NewDesignEvent(
                $"level_complete_count:{levelNumber}",
                1
            );

            GameAnalytics.NewDesignEvent(
                $"level_stars:{levelNumber}",
                starCount
            );

            GameAnalytics.NewDesignEvent(
                $"level_color:{levelNumber}:{activeSheme}"
            );

            Debug.Log($"[GA] Level {levelNumber} complete | time: {seconds} | stars: {starCount} | color: {activeSheme}");
        }

        public void SendLevelFirstTimeComoleteEvent(int levelNumber, float seconds, ColorSheme activeSheme, int starCount)
        {
            GameAnalytics.NewDesignEvent(
                $"level_first_complete:{levelNumber}"
            );
        }

        public void SendAdWatchedEvent(ColorSheme forSheme)
        {
            GameAnalytics.NewAdEvent(
                GAAdAction.Show,
                GAAdType.RewardedVideo,
                "default",
                forSheme.ToString()
            );

            GameAnalytics.NewDesignEvent(
                $"ad_reward_color:{forSheme}"
            );

            Debug.Log($"[GA] Ad watched for color {forSheme}");
        }

        public void SendUnlockColorForAdEvent(ColorSheme forSheme)
        {
            GameAnalytics.NewDesignEvent(
                $"color_unlocked_ad:{forSheme}"
            );
        }

        public void SendSessionEndEvent(int levelsCompleteCount, float playTime)
        {
            GameAnalytics.NewDesignEvent(
                "session_length",
                playTime
            );

            GameAnalytics.NewDesignEvent(
                "session_levels_complete",
                levelsCompleteCount
            );

            Debug.Log($"[GA] Session end | levels: {levelsCompleteCount} | time: {playTime}");
        }

        public void SendRemoveAdsClickedEvent()
        {
            GameAnalytics.NewDesignEvent("remove_ads_clicked");
        }

        public void SendSupportUsScreenShow()
        {
            GameAnalytics.NewDesignEvent(
                $"support_us_screen_showed"
            );
        }

        public void SendSupportUsScreenSupportButtonClicked()
        {
            GameAnalytics.NewDesignEvent(
                $"support_us_screen_support_button_clicked"
            );
        }

        public void SendSupportUsScreenMaybeLaterButtonClicked()
        {
            GameAnalytics.NewDesignEvent(
                $"support_us_screen_later_button_clicked"
            );
        }

        public void SendSupportUsScreenNeverButtonClicked()
        {
            GameAnalytics.NewDesignEvent(
                $"support_us_screen_never_button_clicked"
            );
        }
    }
}