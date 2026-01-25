namespace SanderSaveli.GravityMaze
{
    public class LevelData
    {
        public int Number;
        public int StarCount;
        public LevelStatus Status;

        public LevelData(int number, int starCount, LevelStatus levelStatus)
        {
            Number = number;
            StarCount = starCount;
            Status = levelStatus;
        }
    }
}
