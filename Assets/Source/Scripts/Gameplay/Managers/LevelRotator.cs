using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelRotator : MonoBehaviour
    {
        private IRotationManager _rotationManager;
        private ILevelProvider _levelProvider;
        private Transform _rotationTransform;
        private float _targetRotation;
        private float _zeroRotation;

        [Inject]
        public void Construct(IRotationManager rotationManager, ILevelProvider levelProvider)
        {
            _rotationManager = rotationManager;
            _levelProvider = levelProvider;
        }

        private void OnEnable()
        {
            _rotationTransform = _levelProvider.RotablePart;
            _zeroRotation = _rotationManager.CurrentRotation;
            _rotationManager.OnRotatonChange += HandleRotationChange;
        }

        private void OnDisable()
        {
            _rotationManager.OnRotatonChange -= HandleRotationChange;
        }

        private void HandleRotationChange(float value)
        {
            _targetRotation = _zeroRotation - value;
            Vector3 rotateVector = new Vector3(0, 0, _targetRotation);
            _rotationTransform.rotation = Quaternion.Euler(rotateVector);
        }
    }
}
