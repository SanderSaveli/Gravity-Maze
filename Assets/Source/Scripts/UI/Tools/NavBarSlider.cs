using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class NavBarSlider : MonoBehaviour
    {
        [SerializeField] private NavBarRadioGroup _navBarGroup;
        [SerializeField] private float _sliderSpeed = 0.4f;
        [SerializeField] private float _sliderOffset = 20f;

        private RectTransform _rectTransform;
        private RectTransform _sliderParent;

        private async void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _sliderParent = _rectTransform.parent as RectTransform;

            await UniTask.Yield();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_sliderParent);
            await UniTask.Yield();
            _rectTransform.anchoredPosition = GetLockalPoint();
        }

        private void OnEnable()
        {
            _navBarGroup.OnValueChanged += NewVarianSelect;
        }

        private void OnDisable()
        {
            _navBarGroup.OnValueChanged -= NewVarianSelect;
        }

        private void NewVarianSelect(NavBarOption value)
        {
            Vector2 localPoint = GetLockalPoint();
            _rectTransform.DOAnchorPos(localPoint, _sliderSpeed);
        }

        private Vector2 GetLockalPoint()
        {
            RectTransform target = _navBarGroup.ActiveElement.GetComponent<RectTransform>();

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _sliderParent,
                RectTransformUtility.WorldToScreenPoint(null, target.position),
                null,
                out localPoint
            );
            localPoint.y -= _sliderOffset;
            return localPoint;
        }
    }
}
