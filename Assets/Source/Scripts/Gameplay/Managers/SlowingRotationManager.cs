using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class SlowingRotationManager : MonoBehaviour, IRotationManager
    {
        public float CurrentRotation
        {
            get => _currentRotation;
            private set
            {
                float clamped = Mathf.Clamp(value, 0, MaxRotation);
                if (Math.Abs(clamped - _currentRotation) > 0.001f)
                {
                    _currentRotation = clamped;
                    OnRotatonChange?.Invoke(_currentRotation);
                }
            }
        }

        public Action<float> OnRotatonChange { get; set; }
        public float MaxRotation => 360f;

        [Header("Speed")]
        [SerializeField] private float _secondsBeforeMaxSpeed = 1f;

        [Header("Smoothing")]
        [SerializeField] private float _smoothness = 10f;

        [Header("Deceleration")]
        [SerializeField] private float _decelerationThreshold = 30f;

        private IInputManager _inputManager;

        private float _targetRotation;
        private float _currentRotation;

        private float _currentSpeed;
        private float _maxSpeed;
        private float _acceleration;

        private bool _isHold;

        [Inject]
        public void Construct(IInputManager inputManager, IGameplayConfig config)
        {
            _inputManager = inputManager;
            _maxSpeed = config.RotationSpeed;
            _acceleration = _maxSpeed / _secondsBeforeMaxSpeed;
        }

        private void OnEnable()
        {
            _inputManager.OnPointerDown += StartRotation;
            _inputManager.OnPointerUp += StopRotation;
        }

        private void OnDisable()
        {
            _inputManager.OnPointerDown -= StartRotation;
            _inputManager.OnPointerUp -= StopRotation;
        }

        private void Update()
        {
            float targetSpeed = _isHold ? _maxSpeed : 0f;

            _currentSpeed = Mathf.MoveTowards(
                _currentSpeed,
                targetSpeed,
                _acceleration * Time.deltaTime
            );

            float distanceToEdge = _currentSpeed > 0
                ? MaxRotation - _targetRotation
                : _targetRotation;

            if (distanceToEdge < _decelerationThreshold)
            {
                float t = distanceToEdge / _decelerationThreshold;
                t = Mathf.SmoothStep(0, 1, t);
                _currentSpeed *= t;
            }

            if (_isHold)
            {
                _targetRotation += _currentSpeed * Time.deltaTime;
            }
            else
            {
                _targetRotation = Mathf.MoveTowards(
                    _targetRotation,
                    0,
                    _maxSpeed * Time.deltaTime
                );
            }

            _targetRotation = Mathf.Clamp(_targetRotation, 0, MaxRotation);

            CurrentRotation = Mathf.LerpAngle(
                _currentRotation,
                _targetRotation,
                _smoothness * Time.deltaTime
            );
        }

        private void StartRotation(Vector2 pos)
        {
            _isHold = true;
        }

        private void StopRotation(Vector2 pos)
        {
            _isHold = false;
        }
    }
}