using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameStarView : MonoBehaviour
    {
        [SerializeField] private GameStar _gameStar;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Properties")]
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private float _targetScale = 2.5f;
        [SerializeField] private VibrationType _vibrationType;
        private IVibrationManager _vibrationManager;

        [Inject]
        public void Construct(IVibrationManager vibrationManager)
        {
            _vibrationManager = vibrationManager;
        }

        private void OnEnable()
        {
            if (_gameStar == null)
            {
                Debug.LogError("Game Star is not assigned");
                return;
            }
            _gameStar.OnCollected += HandleCollect;
        }

        private void OnDisable()
        {
            if (_gameStar == null)
            {
                Debug.LogError("Game Star is not assigned");
                return;
            }
            _gameStar.OnCollected -= HandleCollect;
        }

        private void HandleCollect()
        {
            _vibrationManager.DoVibration(_vibrationType);
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_spriteRenderer.DOFade(0, _animationDuration))
                .SetEase(_ease)
                .Join(transform.DOScale(_targetScale, _animationDuration)
                .SetEase(Ease.InOutBack)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false)));
        }
    }
}
