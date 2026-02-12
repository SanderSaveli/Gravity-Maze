using System;

namespace SanderSaveli.GravityMaze
{
    public interface IRotationManager
    {
        public float CurrentRotation { get; }
        public float MaxRotation { get; }
        public Action<float> OnRotatonChange { get; set; }
    }
}
