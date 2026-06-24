using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelSessionMetric : MonoBehaviour
    {
        private IAnalyticManager _analyticManager;
        private SignalBus _signalBus;

        private int _levelsCompleted;
        private float _sessionStartTime;
        private bool _sessionSent;

        [Inject]
        public void Construct(SignalBus signalBus, IAnalyticManager analyticManager)
        {
            _analyticManager = analyticManager;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(GameEnd);
            StartNewSession();
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(GameEnd);
        }

        private void GameEnd(SignalGameEnd ctx)
        {
            if (ctx.IsWin)
            {
                _levelsCompleted++;
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SessionEnd();
            }
            else
            {
                StartNewSession();
            }
        }

        private void OnApplicationQuit()
        {
            SessionEnd();
        }

        private void StartNewSession()
        {
            _sessionSent = false;
            _sessionStartTime = Time.time;
            _levelsCompleted = 0;
        }

        private void SessionEnd()
        {
            if (_sessionSent)
                return;

            _sessionSent = true;

            float playtime = Time.time - _sessionStartTime;

            _analyticManager.SendSessionEndEvent(_levelsCompleted, playtime);

            _levelsCompleted = 0;
        }
    }
}