using System;

namespace SanderSaveli.GravityMaze
{
    public interface IRotationManager
    {
        public float CurrentRotation { get; }
        public Action<float> OnRotatonChange { get; set; }
    }
}
