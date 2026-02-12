using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public abstract class PressHandler : MonoBehaviour
    {
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

            if (delta > 0f && _manager.CurrentRotation != _manager.MaxRotation)
            {
                UpdateMove(delta);
            }
            else
            {
                StopMove();
            }

            _previousAngle = angle;
        }

        protected abstract void UpdateMove(float delta);

        protected abstract void StopMove();
    }
}
