using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class RotatorChangeWhenExitContour : MonoBehaviour
    {
        [SerializeField] private LevelRotator _levelRotator;
        [SerializeField] private CameraLevelRotator _cameraRotator;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _cameraRotator.gameObject.SetActive(true);
            _levelRotator.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalPlayerExitContour>(HandleExitContour);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalPlayerExitContour>(HandleExitContour);
        }

        private void HandleExitContour(SignalPlayerExitContour ctx)
        {
            _cameraRotator.gameObject.SetActive(false);
            _levelRotator.gameObject.SetActive(true);
        }
    }
}
