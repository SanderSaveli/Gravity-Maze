namespace SanderSaveli.GravityMaze
{
    public interface IAnalyticManager
    {
        public void SendLevelCompleteEvent(int levelNumber, float seconds);
    }
}
