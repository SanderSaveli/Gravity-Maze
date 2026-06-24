namespace SanderSaveli.GravityMaze
{
    public class AdReward
    {
        public string Type { get; }
        public double Amount { get; }

        public AdReward(string type = "UnityAds", double amount = 1)
        {
            Type = type;
            Amount = amount;
        }
    }
}
