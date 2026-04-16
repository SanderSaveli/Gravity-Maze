using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class FirebaseInitializer : MonoBehaviour, IAnalyticManager
    {
        private void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependencyStatusRecived);
        }

        public void OnDependencyStatusRecived(Task<DependencyStatus> status)
        {
            try
            {
                if (!status.IsCompletedSuccessfully)
                {
                    throw new Exception("Could not recive all Firebase dependencies " + status.Exception);
                }

                if (status.Result != DependencyStatus.Available)
                {
                    throw new Exception($"Could not recive all Firebase dependencies: {status.Result}");
                }

                Debug.Log("Firebase initialized successfully!");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void SendLevelCompleteEvent(int levelNumber, float seconds, ColorSheme activeSheme, int starCount)
        {
            FirebaseAnalytics.LogEvent(
                "level_complete",
                new Parameter("level_number", levelNumber),
                new Parameter("completion_time", seconds),
                new Parameter("active_color ", activeSheme.ToString()),
                new Parameter("collected_stars ", starCount)
            );

            Debug.Log($"Send analytic Event level_complete, number: {levelNumber}, seconds: {seconds}, color: {activeSheme}, stars: {starCount}");
        }

        public void SendLevelFirstTimeComoleteEvent(int levelNumber, float seconds, ColorSheme activeSheme, int starCount)
        {
            FirebaseAnalytics.LogEvent(
                "level_first_time_complete",
                new Parameter("level_number", levelNumber),
                new Parameter("completion_time", seconds),
                new Parameter("active_color ", activeSheme.ToString())
            );

            Debug.Log($"Send analytic Event level_first_time_complete, number: {levelNumber}, seconds: {seconds}, color: {activeSheme}, stars: {starCount}");
        }


        public void SendAdWatchedEvent(ColorSheme forSheme)
        {
            FirebaseAnalytics.LogEvent(
                "ads_watched_for_color",
                new Parameter("color ", forSheme.ToString())
            );

            Debug.Log($"Send analytic Event ads_watched_for_color, ColorSheme: {forSheme}");
        }

        public void SendSessionEndEvent(int levelsCompleteCount, float playTime)
        {
            FirebaseAnalytics.LogEvent(
                "game_session_end",
                new Parameter("completed_levels ", levelsCompleteCount),
                new Parameter("play_time ", playTime)
            );

            Debug.Log($"Send analytic Event game_session_end, levelsComplete: {levelsCompleteCount}, playTime: {playTime}");
        }

        public void SendRemoveAdsClickedEvent()
        {
            FirebaseAnalytics.LogEvent(
                "remove_ads_clicked"
            );

            Debug.Log($"Send analytic Event remove_ads_clicked");
        }

        public void SendUnlockColorForAdEvent(ColorSheme forSheme)
        {
            FirebaseAnalytics.LogEvent(
                "unlock_color_for_ads",
                new Parameter("unlocked_color", forSheme.ToString())
            );

            Debug.Log($"Send analytic Event unlocked_color, Color: {forSheme}");
        }
    }
}
