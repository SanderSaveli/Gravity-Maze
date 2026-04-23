using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        public IReadOnlyList<ILevelProvider> Levels => _levels.ToList<ILevelProvider>();
        public IReadOnlyList<LevelData> LevelsData => _levelsData;
        public int CurrentLevel => _storage.CurrentLevel.Value;

        [SerializeField] private List<LevelProvider> _levels;
        private List<LevelData> _levelsData;
        private ILevelStorage _storage;
        private CompositeDisposable _disposables;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(ILevelStorage levelStorage, SignalBus signalBus)
        {
            _storage = levelStorage;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _disposables = new CompositeDisposable();
            ActualizeData();
            _storage.Levels.Subscribe((t) => ActualizeData()).AddTo(_disposables);
            _storage.CurrentLevel.Subscribe((t) => ActualizeData()).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
            _disposables = null;
        }

        public ILevelProvider GetCurrentLevel() => GetLevel(CurrentLevel);

        public ILevelProvider GetLevel(int level) => _levels[level];

        public void CompleteLevel(int level, bool isStarCollected)
        {
            int nextLevel = Mathf.Clamp(level + 1, 0, _levels.Count - 1);

            if (nextLevel > CurrentLevel)
            {
                _storage.CurrentLevel.Value = nextLevel;
            }

            if (isStarCollected && _storage.Levels.Value[level].star_count == 0)
            {
                _storage.Levels.Value[level].star_count = 1;
                _storage.Levels.ForceNotify();
                _signalBus.Fire(new SignalStarCountIncrease(_storage.StarCount));
            }
        }

        public void UnlockAllLevels()
        {
            _storage.CurrentLevel.Value = _storage.Levels.Value.Count -1;
        }

        private void ActualizeData()
        {
            if (_storage.Levels.Value == null)
                return;

            if (_levelsData == null)
            {
                InitializeLevelsData();
            }
            else
            {
                UpdateLevelsData();
            }
        }

        private void InitializeLevelsData()
        {
            _levelsData = new List<LevelData>(_levels.Count);

            for (int i = 0; i < _levels.Count; i++)
            {
                _levelsData.Add(CreateLevelData(i));
            }
        }

        private void UpdateLevelsData()
        {
            for (int i = 0; i < _levelsData.Count; i++)
            {
                UpdateLevelData(_levelsData[i], i);
            }
        }

        private void UpdateLevelData(LevelData data, int index)
        {
            data.StarCount = _storage.Levels.Value[index].star_count;
            if (index < CurrentLevel)
                data.Status = LevelStatus.Complete;
            else if (index == CurrentLevel)
                data.Status = LevelStatus.Current;
            else
                data.Status = LevelStatus.Locked;
        }

        private LevelData CreateLevelData(int number)
        {
            LevelStatus levelStatus = LevelStatus.Locked;
            if (number < CurrentLevel)
            {
                levelStatus = LevelStatus.Complete;
            }
            else if (number == CurrentLevel)
            {
                levelStatus = LevelStatus.Current;
            }

            LevelData data = new LevelData(number + 1, _storage.Levels.Value[number].star_count, levelStatus);
            return data;
        }
    }
}
