using R3;
using System;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public class AccountStorage : ILevelStorage, IDisposable, IAdsPurchasizeStorage
    {
        public Action OnUpdate { get; set; }
        public ReactiveProperty<int> CurrentLevel { get; private set; }
        public ReactiveProperty<List<LevelSaveData>> Levels { get; private set; }

        public int StarCount => CalculateStars();

        private Dictionary<ColorSheme, int> _watchedAdsPerColor;

        private CompositeDisposable _disposables;

        public AccountStorage()
        {
            _disposables = new CompositeDisposable();
            CurrentLevel = new ReactiveProperty<int>();
            Levels = new ReactiveProperty<List<LevelSaveData>>();
            _watchedAdsPerColor = new Dictionary<ColorSheme, int>();
        }

        public void SetData(AccountData accountData)
        {
            CurrentLevel.Value = accountData.currentLevel;
            CurrentLevel.Subscribe(Update).AddTo(_disposables);

            Levels.Value = accountData.levels;
            Levels.Subscribe(Update).AddTo(_disposables);

            _watchedAdsPerColor = accountData.watchedAdsPerColor;
        }

        public AccountData GetActualData()
        {
            return new AccountData
            {
                currentLevel = CurrentLevel.Value,
                levels = new List<LevelSaveData>(Levels.Value),
                watchedAdsPerColor = _watchedAdsPerColor
            };
        }

        private void Update<T>(T data)
        {
            OnUpdate?.Invoke();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _disposables = null;
        }

        private int CalculateStars()
        {
            int stars = 0;
            foreach (var level in Levels.Value)
            {
                stars += level.star_count;
            }
            return stars;
        }

        public int GetWatchedAdsPerColor(ColorSheme color)
        {
            if (_watchedAdsPerColor.ContainsKey(color))
            {
                return _watchedAdsPerColor[color];
            }
            else
            {
                return 0;
            }
        }

        public void AddWatch(ColorSheme color)
        {
            if (_watchedAdsPerColor.ContainsKey(color))
            {
                _watchedAdsPerColor[color] = ++_watchedAdsPerColor[color];
            }
            else
            {
                _watchedAdsPerColor.Add(color, 1);
            }
            OnUpdate?.Invoke();
        }
    }
}
