namespace SanderSaveli.GravityMaze
{
    public interface IAdsPurchasizeStorage
    {
        public int GetWatchedAdsPerColor(ColorSheme color);

        public void AddWatch(ColorSheme color);
    }
}
