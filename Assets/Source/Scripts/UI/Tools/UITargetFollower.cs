using DG.Tweening;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class UITargetFollower : MonoBehaviour
    {
        [SerializeField] protected float _targetSpeed = 0.4f;
        [SerializeField] protected Vector2 _sliderOffset;

        protected RectTransform _rectTransform;
        protected RectTransform _sliderParent;

        public void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _sliderParent = _rectTransform.parent as RectTransform;
        }

        public void MoveTo(RectTransform target)
        {
            Debug.Log("Move " + gameObject.name);
            Vector2 localPoint = GetLockalPoint(target);
            _rectTransform.DOAnchorPos(localPoint, _targetSpeed);
        }

        public void MoveToImmediately(RectTransform target)
        {
            Debug.Log("Move Immediately " + gameObject.name);
            Vector2 localPoint = GetLockalPoint(target);
            _rectTransform.anchoredPosition = localPoint;
        }

        private Vector2 GetLockalPoint(RectTransform target)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _sliderParent,
                RectTransformUtility.WorldToScreenPoint(null, target.position),
                null,
                out localPoint
            );
            localPoint += _sliderOffset;
            return localPoint;
        }
    }
}
