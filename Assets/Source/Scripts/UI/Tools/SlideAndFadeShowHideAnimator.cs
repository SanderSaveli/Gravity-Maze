using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(RectTransform))]
    public class SlideAndFadeShowHideAnimator : ShowHideAnimation
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("Show")]
        [SerializeField] private SlideDirection _enterFrom = SlideDirection.Left;
        [SerializeField] private Ease _enterEase = Ease.OutCubic;
        [Header("Hide")]
        [SerializeField] private SlideDirection _exitTo = SlideDirection.Right;
        [SerializeField] private Ease _exitEase = Ease.InCubic;

        [Space]
        [SerializeField] private float offsetMultiplier = 1.2f;

        private RectTransform _rectTransform;
        private Vector2 _initialAnchoredPosition;

        private void Awake()
        {
            if (_rectTransform == null)
            {
                Intialisze();
            }
        }

        private async void Intialisze()
        {
            transform.localScale = Vector3.one;
            _rectTransform = GetComponent<RectTransform>();
            _initialAnchoredPosition = _rectTransform.anchoredPosition;
            await UniTask.Yield();
            foreach (var rt in _rectTransform.GetComponentsInChildren<RectTransform>(true))
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            }
            Debug.Log(_initialAnchoredPosition);
        }

        public override void Hide(float delay, float duration, Action callback)
        {
            Vector2 toPos = GetOffsetPosition(_exitTo);

            Animate(0, toPos, duration, delay, _exitEase, callback);
        }

        public override void HideImmediately()
        {
            if (_rectTransform == null)
            {
                Intialisze();
            }
            _rectTransform.anchoredPosition = GetOffsetPosition(_enterFrom);
            _canvasGroup.alpha = 0;
        }

        public override void Show(float delay, float duration, Action callback)
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            Vector2 fromPos = GetOffsetPosition(_enterFrom);
            _rectTransform.anchoredPosition = fromPos;
            Animate(1, _initialAnchoredPosition, duration, delay, _enterEase, callback);
        }

        public override void ShowImmediately()
        {
            transform.localScale = Vector3.one;
            _rectTransform.anchoredPosition = _initialAnchoredPosition;
            _canvasGroup.alpha = 1;
        }

        private void Animate(float alpha, Vector2 anchoredPosition, float duration, float delay, Ease ease, Action callback)
        {
            transform.localScale = Vector3.one;
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_canvasGroup.DOFade(alpha, duration).SetLink(gameObject))
                .Join(_rectTransform.DOAnchorPos(anchoredPosition, duration))
                .SetEase(ease)
                .SetDelay(delay)
                .SetUpdate(true)
                .OnComplete(() => callback?.Invoke())
                .SetLink(gameObject);
        }

        private Vector2 GetOffsetPosition(SlideDirection direction)
        {
            Vector2 offset = Vector2.zero;
            Vector2 canvasSize = GetCanvasSize();

            switch (direction)
            {
                case SlideDirection.Left:
                    offset = new Vector2(-canvasSize.x * offsetMultiplier, _initialAnchoredPosition.y);
                    break;
                case SlideDirection.Right:
                    offset = new Vector2(canvasSize.x * offsetMultiplier, _initialAnchoredPosition.y);
                    break;
                case SlideDirection.Top:
                    offset = new Vector2(_initialAnchoredPosition.x, canvasSize.y * offsetMultiplier);
                    break;
                case SlideDirection.Bottom:
                    offset = new Vector2(_initialAnchoredPosition.x, -canvasSize.y * offsetMultiplier);
                    break;
            }

            return offset;
        }

        private Vector2 GetCanvasSize()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null && canvas.pixelRect != null)
            {
                return canvas.pixelRect.size;
            }
            return new Vector2(Screen.width, Screen.height);
        }
    }
}
