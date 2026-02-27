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

        public void SendLevelCompleteEvent(int levelNumber, float seconds)
        {
            FirebaseAnalytics.LogEvent(
                "level_complete",
                new Parameter("level_number", levelNumber),
                new Parameter("completion_time", seconds)
            );

            Debug.Log($"Send analytic Event level_complete, number: {levelNumber}, seconds: {seconds}");
        }
    }
}
