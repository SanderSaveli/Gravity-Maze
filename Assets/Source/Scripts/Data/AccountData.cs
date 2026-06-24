using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Rendering;

namespace SanderSaveli.GravityMaze
{
    [Serializable]
    public class AccountData
    {
        public int currentLevel;
        public List<LevelSaveData> levels;
        public Dictionary<ColorSheme, int> watchedAdsPerColor;

        public AccountData() 
        {
            levels = new List<LevelSaveData>();
            watchedAdsPerColor = new Dictionary<ColorSheme, int>();
        }
    }
}
