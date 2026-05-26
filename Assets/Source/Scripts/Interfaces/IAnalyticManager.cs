namespace SanderSaveli.GravityMaze
{
    public interface IAnalyticManager
    {
        public void SendLevelFirstTimeComoleteEvent(int levelNumber, float seconds, ColorSheme activeSheme, int starCount);
        public void SendLevelStartEvent(int levelNumber);
        public void SendLevelCompleteEvent(int levelNumber, float seconds, ColorSheme activeSheme, int starCount);
        public void SendAdWatchedEvent(ColorSheme forSheme);
        public void SendSessionEndEvent(int levelsCompleteCount, float playTime);
        public void SendRemoveAdsClickedEvent();
        public void SendUnlockColorForAdEvent(ColorSheme forSheme);
    }
}
