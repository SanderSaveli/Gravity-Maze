using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public interface IStarManager
    {
        public bool IsStarCollect { get; }
        public IReadOnlyList<GameStar> CollectedStars { get; }
    }
}
