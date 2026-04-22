using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class UnlockColorScreen : UiScreen
    {
        [Header("Components")]
        [SerializeField] private Button _applyTheme;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _colorPreview;
        [SerializeField] private CanvasGroup _applyThemeCanvasGroup;
        [SerializeField] private CanvasGroup _closeThemeCanvasGroup;
        [SerializeField] private LottiePlayer _firework;
        [SerializeField] private LottiePlayer _lock;

        [Header("Params")]
        [SerializeField] private float _lockDelay = 0.5f;

        private ColorSheme _color;
        private IColorManager _colorManager;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(IColorManager colorManager, SignalBus signalBus)
        {
            _colorManager = colorManager;
            _signalBus = signalBus;
        }

        public void Init(ColorSheme color)
        {
            _color = color;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _closeButton.onClick.AddListener(HandleClose);
            _applyTheme.onClick.AddListener(ApplyColor);
            _applyThemeCanvasGroup.alpha = 1.0f;
            _icon.gameObject.SetActive(true);
            _closeThemeCanvasGroup.alpha = 0;
            _applyThemeCanvasGroup.interactable = true;
            _closeButton.interactable = false;
            _firework.gameObject.SetActive(false);
            _colorPreview.color = _colorManager.GetActiveColorOfSheme(_color);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _closeButton.onClick.RemoveListener(HandleClose);
            _applyTheme.onClick.RemoveListener(ApplyColor);
        }

        public async void ApplyColor()
        {
            _icon.gameObject.SetActive(false);
            _lock.Play();
            _firework.gameObject.SetActive(true);
            _firework.Play();
            _applyThemeCanvasGroup.interactable = false;
            _applyThemeCanvasGroup.DOFade(0, _lockDelay).SetLink(_applyThemeCanvasGroup.gameObject);
            _closeThemeCanvasGroup.DOFade(1, _lockDelay).SetLink(_closeThemeCanvasGroup.gameObject);
            _colorManager.ActiveSheme.Value = _color;
            await UniTask.WaitForSeconds(_lockDelay, true);
            _closeButton.interactable = true;
        }

        private void HandleClose()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Color));
        }
    }
}
