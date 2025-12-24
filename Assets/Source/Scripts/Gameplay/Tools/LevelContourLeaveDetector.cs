using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelContourLeaveDetector : MonoBehaviour
    {
        private ILevelProvider _levelProvider;
        private bool _isLeave;
        private Bounds _bounds;
        private Transform _playerTransform;
        private Player _player;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(ILevelProvider levelProvider, SignalBus signalBus)
        {
            _levelProvider = levelProvider;
            _signalBus = signalBus;
        }

        private void Start()
        {
            _player = _levelProvider.Player;
            _playerTransform = _player.transform;
            CreateBounds();
        }

        private void Update()
        {
            if (_isLeave)
            {
                return;
            }

            if (!_bounds.Contains(_playerTransform.position))
            {
                _signalBus.Fire(new SignalPlayerExitContour(_player));
                _isLeave = true;
            }
        }

        private void CreateBounds()
        {
            _bounds = new Bounds();

            Renderer[] renderers = _levelProvider.RotablePart.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                _bounds.Encapsulate(renderer.bounds);
            }
        }
    }
}
