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
                float clamped = Mathf.Clamp(value, 0f, MaxRotation);

                if (Mathf.Abs(clamped - _currentRotation) > 0.001f)
                {
                    _currentRotation = clamped;
                    OnRotatonChange?.Invoke(_currentRotation);
                }
            }
        }

        public Action<float> OnRotatonChange { get; set; }
        public float MaxRotation => 360f;

        [SerializeField] private float _secondsBeforeMaxSpeed = 0.3f;

        private IInputManager _inputManager;

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
            if (CurrentRotation == MaxRotation || CurrentRotation == 0)
            {
                _currentSpeed = 0;
            }
            float targetSpeed = _isHold ? _maxSpeed : -_maxSpeed;

            _currentSpeed = Mathf.MoveTowards(
                _currentSpeed,
                targetSpeed,
                _acceleration * Time.deltaTime
            );

            CurrentRotation += _currentSpeed * Time.deltaTime;
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