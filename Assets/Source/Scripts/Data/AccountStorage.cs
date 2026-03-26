using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class AccountStorage : ILevelStorage, IDisposable
    {
        public Action OnUpdate { get; set; }
        public ReactiveProperty<int> CurrentLevel { get; private set; }
        public ReactiveProperty<List<LevelSaveData>> Levels { get; private set; }

        public int StarCount => CalculateStars();

        private CompositeDisposable _disposables;

        public AccountStorage()
        {
            _disposables = new CompositeDisposable();
            CurrentLevel = new ReactiveProperty<int>();
            Levels = new ReactiveProperty<List<LevelSaveData>>();
        }

        public void SetData(AccountData accountData)
        {
            CurrentLevel.Value = accountData.currentLevel;
            CurrentLevel.Subscribe(Update).AddTo(_disposables);

            Levels.Value = accountData.levels;
            Levels.Subscribe(Update).AddTo(_disposables);
        }

        public AccountData GetActualData()
        {
            return new AccountData
            {
                currentLevel = CurrentLevel.Value,
                levels = new List<LevelSaveData>(Levels.Value)
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
    }
}
