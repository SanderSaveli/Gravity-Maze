using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class CameraLevelRotator : MonoBehaviour
    {
        private IRotationManager _rotationManager;
        private Transform _camera;

        [Inject]
        public void Construct(IRotationManager rotationManager)
        {
            _rotationManager = rotationManager;
        }

        private float _targetRotation;
        private float _currentRotation;
        private float _lastRotation;

        /// <summary>
        /// “екуща€ углова€ скорость камеры (рад/с)
        /// </summary>
        public float AngularVelocityRad { get; private set; }

        [SerializeField] private float _rotationSpeed = 720f; // град/с

        private void Start()
        {
            _camera = Camera.main.transform;
            _currentRotation = _camera.eulerAngles.z;
            _lastRotation = _currentRotation;
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

        private void LateUpdate()
        {
            _currentRotation = Mathf.MoveTowardsAngle(
                _currentRotation,
                _targetRotation,
                _rotationSpeed * Time.deltaTime
            );

            AngularVelocityRad =
                Mathf.DeltaAngle(_lastRotation, _currentRotation) *
                Mathf.Deg2Rad / Time.deltaTime;

            _lastRotation = _currentRotation;

            _camera.rotation = Quaternion.Euler(0f, 0f, _currentRotation);
        }
    }
}
