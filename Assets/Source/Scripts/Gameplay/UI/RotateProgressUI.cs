using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class RotateProgressUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasScaler _canvas;

        private IRotationManager _rotationManager;
        private float _maxHeight;

        [Inject]
        public void Construct(IRotationManager rotationManager)
        {
            _rotationManager = rotationManager;
        }

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInChildren<CanvasScaler>();
        }

        private void Start()
        {
            _maxHeight = _canvas.referenceResolution.y;
        }

        private void OnEnable()
        {
            _rotationManager.OnRotatonChange += UpdateProgressAmount;
            UpdateProgressAmount(_rotationManager.CurrentRotation);
        }

        private void OnDisable()
        {
            _rotationManager.OnRotatonChange -= UpdateProgressAmount;
        }

        private void UpdateProgressAmount(float amount)
        {
            float t = amount / _rotationManager.MaxRotation;
            float currAmount = t * _maxHeight;
            if (currAmount == 0)
            {
                currAmount = -1;
            }
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, currAmount);
        }
    }
}
