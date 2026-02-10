using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class PressRotate : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;

        private IRotationManager _manager;
        private float _previousAngle;

        [Inject]
        public void Construct(IRotationManager rotationManager)
        {
            _manager = rotationManager;
        }

        private void OnEnable()
        {
            _previousAngle = 0f;
            _manager.OnRotatonChange += Rotate;
        }

        private void OnDisable()
        {
            _manager.OnRotatonChange -= Rotate;
        }

        private void Rotate(float angle)
        {
            float delta = angle - _previousAngle;

            if (delta > 0f)
            {
                ApplyRotation(delta);
            }
            else
            {
                StopRotation();
            }

            _previousAngle = angle;
        }

        private void ApplyRotation(float delta)
        {
            float angularVelocity = _speed * delta;
            _rigidbody.angularVelocity = angularVelocity;
        }

        private void StopRotation()
        {
            _rigidbody.angularVelocity = 0;
        }
    }
}
