using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class NotifficationView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _previewImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;

        [Header("Params")]
        [SerializeField] private float _showTime = 1;
        [SerializeField] private float _animationDuration = 0.5f;
        [SerializeField] private float _verticalOffset = 20;

        private Vector2 _normalPos;
        private Vector2 _initialPos;

        private void OnEnable()
        {
            _canvasGroup.alpha = 0;
        }

        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            _normalPos = _rectTransform.anchoredPosition;
            _initialPos = _normalPos;
            _initialPos.y += _verticalOffset;
            gameObject.SetActive(false);
        }

        public void ShowNewColor(Color color)
        {
            gameObject.SetActive(true);
            _previewImage.color = color;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

            _canvasGroup.alpha = 0;
            _rectTransform.anchoredPosition = _initialPos;

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_canvasGroup.DOFade(1, _animationDuration))
                .Join(_rectTransform.DOAnchorPos(_normalPos, _animationDuration))
                .AppendInterval(_showTime)
                .Append(_canvasGroup.DOFade(0, _animationDuration))
                .Join(_rectTransform.DOAnchorPos(_initialPos, _animationDuration))
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
