using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class LevelProvider : MonoBehaviour, ILevelProvider
    {
        public Player Player => _player;
        public Transform RotablePart => _rotatablePart;

        [SerializeField] private Player _player;
        [SerializeField] private Transform _rotatablePart;
    }
}
