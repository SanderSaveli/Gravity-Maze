using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelComplectionMetric : MonoBehaviour
    {
        private const string SavedLevelKey = "metric_saved_level";
        private const string SavedTimeKey = "metric_saved_time";

        private IAnalyticManager _analyticManager;
        private IGameContext _gameContext;
        private IColorManager _colorManager;
        private ILevelManager _levelManager;
        private IStarManager _starManager;
        private SignalBus _signalBus;

        private float _timeToComplete;
        private int _currentLevel;
        private bool _isRunning;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            IAnalyticManager analyticManager,
            IGameContext gameContext, 
            IColorManager colorManager, 
            ILevelManager levelManager,
            IStarManager starManager)
        {
            _signalBus = signalBus;
            _analyticManager = analyticManager;
            _gameContext = gameContext;
            _colorManager = colorManager;
            _levelManager = levelManager;
            _starManager = starManager;
        }

        private void Start()
        {
            _currentLevel = _gameContext.LevelNumber;

            int savedLevel = PlayerPrefs.GetInt(SavedLevelKey, -1);

            if (savedLevel == _currentLevel)
            {
                _timeToComplete = PlayerPrefs.GetFloat(SavedTimeKey, 0f);
                Debug.Log("Loaded from level");
                Debug.Log(savedLevel);
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
                if(_levelManager.CurrentLevel == _gameContext.LevelNumber)
                {
                    _analyticManager.SendLevelFirstTimeComoleteEvent(_currentLevel + 1, _timeToComplete, _colorManager.ActiveSheme.Value, _starManager.CollectedStars.Count);
                }
                _analyticManager.SendLevelCompleteEvent(_currentLevel + 1, _timeToComplete, _colorManager.ActiveSheme.Value, _starManager.CollectedStars.Count);

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
            PlayerPrefs.SetInt(SavedLevelKey, -1);
            PlayerPrefs.SetFloat(SavedTimeKey, 0);
            PlayerPrefs.Save();
        }
    }
}