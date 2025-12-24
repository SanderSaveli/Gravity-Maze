namespace SanderSaveli.GravityMaze
{
    public readonly struct SignalGameEnd
    {
        public readonly bool IsWin;

        public SignalGameEnd(bool isWin)
        {
            IsWin = isWin;
        }
    }
}
