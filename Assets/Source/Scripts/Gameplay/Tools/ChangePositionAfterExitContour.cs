using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ChangePositionAfterExitContour : MonoBehaviour
    {
        [SerializeField] private Transform _targetParent;
        private SignalBus _signalBus;
        private GameObject _deltaObject;
        private Transform _player;
        private Vector3 _previousPosition;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void LateUpdate()
        {
            if(_player!= null && _deltaObject != null)
            {
                Vector3 delta = _deltaObject.transform.position - _previousPosition;
                _player.position = _player.position + delta;
                _deltaObject.transform.position = _player.position;
                _previousPosition = _deltaObject.transform.position;
            }
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
            _player = ctx.Player.transform;
            _deltaObject = new GameObject("DeltaPosOBJ");
            _deltaObject.transform.SetParent(_targetParent);
            _deltaObject.transform.position = _player.position;
            _previousPosition = _player.position;
        }
    }
}
