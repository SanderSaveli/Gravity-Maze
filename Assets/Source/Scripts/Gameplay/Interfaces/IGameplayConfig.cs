namespace SanderSaveli.GravityMaze
{
    public interface IGameplayConfig
    {
        public float GravityForce { get; set; }
        public float Friction { get; set; }
        public float Bounciness { get; set; }
        public float RotationSpeed { get; set; }
    }
}
