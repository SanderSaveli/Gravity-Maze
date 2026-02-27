using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelComplectionMetric : MonoBehaviour
    {
        private const string SavedLevelKey = "metric_saved_level";
        private const string SavedTimeKey = "metric_saved_time";

        private IAnalyticManager _analyticManager;
        private ILevelManager _levelManager;
        private SignalBus _signalBus;

        private float _timeToComplete;
        private int _currentLevel;
        private bool _isRunning;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            IAnalyticManager analyticManager,
            ILevelManager levelManager)
        {
            _signalBus = signalBus;
            _analyticManager = analyticManager;
            _levelManager = levelManager;
        }

        private void Start()
        {
            _currentLevel = _levelManager.CurrentLevel;

            int savedLevel = PlayerPrefs.GetInt(SavedLevelKey, -1);

            if (savedLevel == _currentLevel)
            {
                _timeToComplete = PlayerPrefs.GetFloat(SavedTimeKey, 0f);
            }
            else
            {
                _timeToComplete = 0f;
                SaveProgress();
            }

            _isRunning = true;
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveProgress();
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }

        private void Update()
        {
            if (!_isRunning)
                return;

            _timeToComplete += Time.deltaTime;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
            _signalBus.Subscribe<SignalInputAction>(SaveProgress);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
            _signalBus.Unsubscribe<SignalInputAction>(SaveProgress);
        }

        private void HandleGameEnd(SignalGameEnd ctx)
        {
            _isRunning = false;
            if (ctx.IsWin)
            {
                _analyticManager.SendLevelCompleteEvent(_currentLevel +1, _timeToComplete);

                ClearSavedProgress();
            }
            else
            {
                SaveProgress();
            }
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetInt(SavedLevelKey, _currentLevel);
            PlayerPrefs.SetFloat(SavedTimeKey, _timeToComplete);
            PlayerPrefs.Save();
        }

        private void ClearSavedProgress()
        {
            PlayerPrefs.DeleteKey(SavedLevelKey);
            PlayerPrefs.DeleteKey(SavedTimeKey);
            PlayerPrefs.Save();
        }
    }
}