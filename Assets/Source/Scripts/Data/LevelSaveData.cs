using System;

namespace SanderSaveli.GravityMaze
{
    [Serializable]
    public class LevelSaveData
    {
        public int star_count;

        public LevelSaveData(int star_count)
        {
            this.star_count = star_count;
        }
    }
}
