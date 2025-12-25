using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class CascadeShowHideAnimator : ShowHideAnimation
    {
        private class BlockContext
        {
            public CanvasGroup CanvasGroup;
            public RectTransform RectTransform;
            public Vector2 InitialPosition;
        }

        [Header("General")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float offsetMultiplier = 1.2f;

        [Header("Show")]
        [SerializeField] private SlideDirection _enterFrom = SlideDirection.Left;
        [SerializeField] private Ease _enterEase = Ease.OutCubic;
        [Header("Hide")]
        [SerializeField] private SlideDirection _exitTo = SlideDirection.Right;
        [SerializeField] private Ease _exitEase = Ease.InCubic;

        private List<BlockContext> _blocks;

        private void Reset()
        {
            _rectTransform = transform.GetComponent<RectTransform>();
        }

        private void Awake()
        {
            Intialisze();
        }

        private async void Intialisze()
        {
            transform.localScale = Vector3.one;
            EnshureCanvases();
            await UniTask.Yield();
            foreach (var rt in _rectTransform.GetComponentsInChildren<RectTransform>(true))
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            }
        }

        public override void Hide(float delay, float duration, Action callback)
        {
            float stepDelay = duration / _blocks.Count;
            Debug.Log("Duration: "+ duration);
            for (int i = 0; i < _blocks.Count; i++)
            {
                Debug.Log("Block");
                BlockContext blockContext = _blocks[i];
                Vector2 toOffset = GetOffsetPosition(_exitTo, blockContext);
                blockContext.RectTransform.DOAnchorPos(toOffset, duration)
                    .SetEase(_exitEase)
                    .SetDelay(delay)
                    .SetUpdate(true)
                    .SetLink(gameObject);

                blockContext.CanvasGroup.DOFade(0, duration)
                    .SetDelay(delay)
                    .SetUpdate(true)
                    .SetLink(gameObject);

                delay += stepDelay;
            }

            DOVirtual.DelayedCall(delay, () => callback?.Invoke())
                .SetLink(gameObject);
        }

        public override void HideImmediately()
        {
            if(_blocks == null)
            {
                Intialisze();
            }
            foreach (var block in _blocks)
            {
                block.CanvasGroup.alpha = 0;
            }
        }

        public override void Show(float delay, float duration, Action callback)
        {
            float stepDelay = duration / _blocks.Count;

            for (int i = 0; i < _blocks.Count; i++)
            {
                BlockContext blockContext = _blocks[i];
                Vector2 fromOffset = GetOffsetPosition(_enterFrom, blockContext);
                blockContext.RectTransform.anchoredPosition = fromOffset;
                blockContext.CanvasGroup.alpha = 0;

                blockContext.RectTransform.DOAnchorPos(blockContext.InitialPosition, duration)
                    .SetEase(_enterEase)
                    .SetDelay(delay)
                    .SetUpdate(true)
                    .SetLink(gameObject);

                blockContext.CanvasGroup.DOFade(1, duration)
                    .SetDelay(delay)
                    .SetUpdate(true)
                    .SetLink(gameObject);

                delay += stepDelay;
            }

            DOVirtual.DelayedCall(delay, () => callback?.Invoke())
                .SetLink(gameObject);
        }


        public override void ShowImmediately()
        {
            foreach(var block in _blocks)
            {
                block.CanvasGroup.alpha = 1;
                block.RectTransform.anchoredPosition = block.InitialPosition;
            }
        }

        private void Animate(Vector2 anchoredPosition, float duration, float delay, Ease ease, Action callback)
        {
            transform.localScale = Vector3.one;
            _rectTransform.DOAnchorPos(anchoredPosition, duration)
                .SetEase(ease)
                .SetDelay(delay)
                .SetUpdate(true)
                .OnComplete(() => callback?.Invoke())
                .SetLink(gameObject);
        }

        private Vector2 GetOffsetPosition(SlideDirection direction, BlockContext block)
        {
            Vector2 offset = Vector2.zero;
            Vector2 canvasSize = GetCanvasSize();

            switch (direction)
            {
                case SlideDirection.Left:
                    offset = new Vector2(block.InitialPosition.x - canvasSize.x * offsetMultiplier, block.InitialPosition.y);
                    break;
                case SlideDirection.Right:
                    offset = new Vector2(block.InitialPosition.x + canvasSize.x * offsetMultiplier, block.InitialPosition.y);
                    break;
                case SlideDirection.Top:
                    offset = new Vector2(block.InitialPosition.x, block.InitialPosition.y + canvasSize.y * offsetMultiplier);
                    break;
                case SlideDirection.Bottom:
                    offset = new Vector2(block.InitialPosition.x, block.InitialPosition.y - canvasSize.y * offsetMultiplier);
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

        private void EnshureCanvases()
        {
            int childCount = _rectTransform.childCount;
            _blocks = new List<BlockContext>();

            for (int i = 0; i < childCount; i++)
            {
                RectTransform child = _rectTransform.GetChild(i) as RectTransform;

                if (!child.TryGetComponent(out CanvasGroup group))
                    group = child.AddComponent<CanvasGroup>();

                BlockContext blockContext = new BlockContext();
                blockContext.CanvasGroup = group;
                blockContext.RectTransform = child;
                blockContext.InitialPosition = child.anchoredPosition;
                _blocks.Add(blockContext);
            }
        }

    }
}
