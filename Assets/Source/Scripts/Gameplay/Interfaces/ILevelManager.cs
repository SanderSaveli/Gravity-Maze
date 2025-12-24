using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public interface ILevelManager
    {
        public IReadOnlyList<ILevelProvider> Levels { get; }
        public int CurrentLevel { get; }
        public ILevelProvider GetLevel(int level);
        public ILevelProvider GetCurrentLevel();
        public void CompleteLevel(int level);
    }
}
