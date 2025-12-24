using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class PhysicLevelRotator : MonoBehaviour
    {
        private IRotationManager _rotationManager;
        private ILevelProvider _levelProvider;

        private Rigidbody2D _rb;
        private float _targetRotation;

        [Inject]
        public void Construct(IRotationManager rotationManager, ILevelProvider levelProvider)
        {
            _rotationManager = rotationManager;
            _levelProvider = levelProvider;
        }

        private void Awake()
        {
            _rb = _levelProvider.RotablePart.GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rotationManager.OnRotatonChange += HandleRotationChange;
        }

        private void OnDisable()
        {
            _rotationManager.OnRotatonChange -= HandleRotationChange;
        }

        private void HandleRotationChange(float value)
        {
            _targetRotation = value;
        }

        private void FixedUpdate()
        {
            Quaternion target = Quaternion.Euler(0f, 0f, _targetRotation);
            _rb.MoveRotation(target);
        }
    }
}