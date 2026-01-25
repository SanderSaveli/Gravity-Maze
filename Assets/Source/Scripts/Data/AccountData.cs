using System;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    [Serializable]
    public class AccountData
    {
        public int currentLevel;
        public List<LevelSaveData> levels;

        public AccountData() 
        {
            levels = new List<LevelSaveData>();
        }
    }
}
