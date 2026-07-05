using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class UIFlyToTargetAnimator : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform _movingObjectPrefab;
        [SerializeField] private RectTransform _movingParent;
        [SerializeField] private Image _sourceImage;

        [Header("Parameters")]
        [SerializeField] private float _moveDuration = 1.2f;
        [SerializeField] private Ease _moveEase = Ease.InOutSine;
        [SerializeField] private bool _useUnscaledTime = true;

        public void Play(RectTransform startPoint, RectTransform target, Action onComplete)
        {
            if (startPoint == null || target == null)
            {
                Debug.LogError("UI fly animator has no start point or target.");
                return;
            }

            RectTransform movingObject = CreateMovingObject(startPoint);
            movingObject.position = startPoint.position;
            movingObject.rotation = startPoint.rotation;
            movingObject.localScale = startPoint.localScale;

            movingObject
                .DOMove(target.position, _moveDuration)
                .SetEase(_moveEase)
                .SetUpdate(_useUnscaledTime)
                .SetLink(movingObject.gameObject)
                .SetSpeedBased()
                .OnComplete(() => CompleteMove(movingObject, onComplete));
        }

        private RectTransform CreateMovingObject(RectTransform startPoint)
        {
            Transform parent = _movingParent != null ? _movingParent : startPoint.parent;

            if (_movingObjectPrefab != null)
                return Instantiate(_movingObjectPrefab, parent);

            GameObject movingObject = new GameObject("Flying Animation Object", typeof(RectTransform));
            RectTransform rectTransform = (RectTransform)movingObject.transform;
            rectTransform.SetParent(parent, false);
            rectTransform.sizeDelta = startPoint.sizeDelta;
            rectTransform.pivot = startPoint.pivot;

            movingObject.AddComponent<CanvasRenderer>();
            Image image = movingObject.AddComponent<Image>();
            CopyImageData(image);
            return rectTransform;
        }

        private void CopyImageData(Image targetImage)
        {
            if (_sourceImage == null)
                return;

            targetImage.sprite = _sourceImage.sprite;
            targetImage.color = _sourceImage.color;
            targetImage.type = _sourceImage.type;
            targetImage.preserveAspect = _sourceImage.preserveAspect;
            targetImage.raycastTarget = false;
        }

        private void CompleteMove(RectTransform movingObject, Action onComplete)
        {
            onComplete?.Invoke();
            Destroy(movingObject.gameObject);
        }
    }
}
