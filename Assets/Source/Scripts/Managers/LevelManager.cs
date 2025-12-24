using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        public IReadOnlyList<ILevelProvider> Levels => _levels.ToList<ILevelProvider>();

        public int CurrentLevel => _lastCompleteLevel;

        [SerializeField] private List<LevelProvider> _levels;
        private const string LEVEL_KEY = "Save/Levels/LastComplete";
        private int _lastCompleteLevel;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(LEVEL_KEY))
            {
                PlayerPrefs.SetInt(LEVEL_KEY, 0);
            }
            _lastCompleteLevel = PlayerPrefs.GetInt(LEVEL_KEY);
        }

        public ILevelProvider GetCurrentLevel() => GetLevel(_lastCompleteLevel);

        public ILevelProvider GetLevel(int level) => _levels[level];

        public void CompleteLevel(int level)
        {
            if(level >= _levels.Count)
            {
                _lastCompleteLevel = 0;
            }
            else
            {
                _lastCompleteLevel = level;
            }
            //_lastCompleteLevel = Mathf.Clamp(level, _lastCompleteLevel, _levels.Count - 1);

            PlayerPrefs.SetInt(LEVEL_KEY, _lastCompleteLevel);
        }
    }
}
