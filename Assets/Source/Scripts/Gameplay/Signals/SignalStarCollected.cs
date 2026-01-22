namespace SanderSaveli.GravityMaze
{
    public readonly struct SignalStarCollected
    {
        public readonly GameStar _gameStar;

        public SignalStarCollected(GameStar gameStar)
        {
            _gameStar = gameStar;
        }
    }
}
