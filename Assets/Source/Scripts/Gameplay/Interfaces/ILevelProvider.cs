using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public interface ILevelProvider
    {
        public Player Player { get; }
        public Transform RotablePart { get; }
    }
}
