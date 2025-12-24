using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class RotationManager : MonoBehaviour, IRotationManager
    {
        public float CurrentRotation
        {
            get => _currentRotation;
            private set
            {
                float clamped = Mathf.Clamp(value, 0, 360);
                if(clamped != _currentRotation)
                {
                    _currentRotation = clamped;
                    OnRotatonChange?.Invoke(_currentRotation);
                }
            }
        }
        public Action<float> OnRotatonChange { get; set; }

        [SerializeField] private float _rotationSpeed;

        private IInputManager _inputManager;
        private float _currentRotation;
        private bool _isHold;

        [Inject]
        public void Construct(IInputManager inputManager)
        {
            _inputManager = inputManager;
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
            if(_isHold)
            {
                CurrentRotation += _rotationSpeed * Time.deltaTime;
            }
            else
            {
                CurrentRotation -= _rotationSpeed * Time.deltaTime;
            }
        }
        private void StartRotation(Vector2 screenPosition)
        {
            _isHold = true;
        }

        private void StopRotation(Vector2 screenPosition)
        {
            _isHold = false;
        }
    }
}
