using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [CreateAssetMenu(fileName = "GameplayConfigSO", menuName = "GravityMaze/GameplayConfig")]
    public class GameplayConfigSO : ScriptableObject, IGameplayConfig
    {
        public float GravityForce { get => _gravityForce; set => _gravityForce = value; }
        public float Friction { get => _friction; set => _friction = value; }
        public float Bounciness { get => _bounsness; set => _bounsness = value; }
        public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }

        [SerializeField] private float _gravityForce;
        [SerializeField] private float _friction;
        [SerializeField] private float _bounsness;
        [SerializeField] private float _rotationSpeed;
    }
}
