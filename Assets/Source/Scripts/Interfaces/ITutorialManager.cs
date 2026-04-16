namespace SanderSaveli.GravityMaze
{
    public interface ITutorialManager
    {
        public bool HasTutorialForLevel(int level, out LevelTutorialData data);

        public void CompleteTutorial(int level);
    }
}
