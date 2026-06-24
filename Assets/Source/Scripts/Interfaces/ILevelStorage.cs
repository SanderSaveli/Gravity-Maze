using R3;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public interface ILevelStorage
    {
        public ReactiveProperty<List<LevelSaveData>> Levels { get; }
        public ReactiveProperty<int> CurrentLevel { get; }
        public int StarCount { get; }
    }
}
