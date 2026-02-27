using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameButtonView : MonoBehaviour
    {
        [SerializeField] private GameButton _gameButton;
        [Space]
        [SerializeField] private float _demping = 0.5f;
        [SerializeField] private float _animationDuration = 0.4f;
        [SerializeField] private VibrationType _vibrationType;
        private IVibrationManager _vibrationManager;

        [Inject]
        public void Construct(IVibrationManager vibrationManager)
        {
            _vibrationManager = vibrationManager;
        }

        private void OnEnable()
        {
            _gameButton.OnActive += HandleAnimate;
        }

        private void OnDisable()
        {
            _gameButton.OnActive -= HandleAnimate;
        }

        private void HandleAnimate()
        {
            _vibrationManager.DoVibration(_vibrationType);
            Vector3 nextPos;
            nextPos = transform.position;
            nextPos -= transform.up * _demping;
            transform.DOLocalMove(nextPos, _animationDuration);
        }
    }
}
