namespace SanderSaveli.GravityMaze
{
    public interface ITimeManager
    {
        public float CurrentTimeScale { get; }

        public void ChangeTimeMode(TimeMode timeMode);
    }
}
