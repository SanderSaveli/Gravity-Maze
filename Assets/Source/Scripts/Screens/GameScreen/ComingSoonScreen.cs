using DG.Tweening;
using SanderSaveli.UDK.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ComingSoonScreen : UiScreen
    {
        [Header("Refs")]
        [SerializeField] private Button _button;

        [SerializeField] private RectTransform _block1;
        [SerializeField] private CanvasGroup _block1CanvasGroup;

        [SerializeField] private RectTransform _block2;
        [SerializeField] private CanvasGroup _block2CanvasGroup;

        [Header("Animation")]
        [SerializeField] private float _delayTime = 0.45f;
        [SerializeField] private float _appearTime = 0.45f;
        [SerializeField] private float _delayBetweenBlocks = 0.18f;
        [SerializeField] private Vector2 _offset_1 = new Vector2(0f, -80f);
        [SerializeField] private Vector2 _offset_2 = new Vector2(0f, -80f);
        [SerializeField] private Ease _ease = Ease.OutCubic;

        private SignalBus _signalBus;
        private Sequence _sequence;

        private Vector2 _block1TargetPos;
        private Vector2 _block2TargetPos;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _block1TargetPos = _block1.anchoredPosition;
            _block2TargetPos = _block2.anchoredPosition;
        }

        protected override void SubscribeToEvents()
        {
            _button.onClick.AddListener(HandleExit);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _button.onClick.RemoveListener(HandleExit);
            base.UnsubscribeFromEvents();
        }

        public override void Show(Action callback = null)
        {
            base.Show(callback);
            ShowAnimation();
        }

        public override void Hide(Action callback = null)
        {
            HideAnimation(callback);
        }

        private void HandleExit()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
            Hide();
        }

        private void ShowAnimation()
        {
            KillAnimation();

            PrepareBlock(_block1, _block1CanvasGroup, _block1TargetPos + _offset_1, 0f);
            PrepareBlock(_block2, _block2CanvasGroup, _block2TargetPos + _offset_2, 0f);

            _sequence = DOTween.Sequence()
                .SetUpdate(true)
                .AppendInterval(_delayTime)
                .Append(ShowBlock(_block1, _block1CanvasGroup, _block1TargetPos))
                .AppendInterval(_delayBetweenBlocks)
                .Append(ShowBlock(_block2, _block2CanvasGroup, _block2TargetPos));
        }

        private void HideAnimation(Action callback = null)
        {
            KillAnimation();

            _sequence = DOTween.Sequence()
                .SetUpdate(true)
                .Append(HideBlock(_block2, _block2CanvasGroup, _block2TargetPos + _offset_1))
                .AppendInterval(_delayBetweenBlocks)
                .Append(HideBlock(_block1, _block1CanvasGroup, _block1TargetPos + _offset_2))
                .OnComplete(() =>
                {
                    base.Hide(callback);
                });
        }

        private Tween ShowBlock(RectTransform rect, CanvasGroup canvasGroup, Vector2 targetPosition)
        {
            return DOTween.Sequence()
                .Join(rect.DOAnchorPos(targetPosition, _appearTime).SetEase(_ease))
                .Join(canvasGroup.DOFade(1f, _appearTime).SetEase(_ease));
        }

        private Tween HideBlock(RectTransform rect, CanvasGroup canvasGroup, Vector2 targetPosition)
        {
            return DOTween.Sequence()
                .Join(rect.DOAnchorPos(targetPosition, _appearTime).SetEase(_ease))
                .Join(canvasGroup.DOFade(0f, _appearTime).SetEase(_ease));
        }

        private void PrepareBlock(
            RectTransform rect,
            CanvasGroup canvasGroup,
            Vector2 position,
            float alpha)
        {
            rect.anchoredPosition = position;
            canvasGroup.alpha = alpha;
        }

        private void KillAnimation()
        {
            if (_sequence != null && _sequence.IsActive())
            {
                _sequence.Kill();
                _sequence = null;
            }
        }
    }
}