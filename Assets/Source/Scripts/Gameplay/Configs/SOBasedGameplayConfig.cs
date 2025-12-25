using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class SOBasedGameplayConfig : MonoBehaviour, IGameplayConfig
    {
        [SerializeField] private GameplayConfigSO _initialValues;

        public float GravityForce { get => _gravityForce; set => _gravityForce = value; }
        public float Friction { get => _friction; set => _friction = value; }
        public float Bounciness { get => _bounsness; set => _bounsness = value; }
        public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }

        private float _gravityForce;
        private float _friction;
        private float _bounsness;
        private float _rotationSpeed;

        private void Awake()
        {
            _gravityForce = _initialValues.GravityForce;
            _friction = _initialValues.Friction;
            _bounsness = _initialValues.Bounciness;
            _rotationSpeed = _initialValues.RotationSpeed;
        }
    }
}
