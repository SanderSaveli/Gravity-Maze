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

        [Inject]
        public void Construct(IRotationManager rotationManager, ILevelProvider levelProvider)
        {
            _rotationManager = rotationManager;
            _levelProvider = levelProvider;
        }

        private void Start()
        {
            _rotationTransform = _levelProvider.RotablePart;
        }

        private void OnEnable()
        {
            _rotationManager.OnRotatonChange += HandleRotationChange;
        }

        private void OnDisable()
        {
            _rotationManager.OnRotatonChange -= HandleRotationChange;
        }

        private void FixedUpdate()
        {
            Vector3 rotateVector = new Vector3(0, 0, _targetRotation);
            _rotationTransform.rotation = Quaternion.Euler(rotateVector);
        }

        private void HandleRotationChange(float value)
        {
            _targetRotation = value;
        }
    }
}
