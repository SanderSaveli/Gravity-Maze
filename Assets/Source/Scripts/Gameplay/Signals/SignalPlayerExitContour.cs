namespace SanderSaveli.GravityMaze
{
    public readonly struct SignalPlayerExitContour
    {
        public readonly Player Player;

        public SignalPlayerExitContour(Player player)
        {
            Player = player;
        }
    }
}
