using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LanguageScreen : UiScreen
    {
        [SerializeField] private SelectingSnapScroll _languageSnapScrol;
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _content;
        private ILanguageChanger<Language> _languageChanger;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(ILanguageChanger<Language> language, SignalBus signalBus)
        {
            _languageChanger = language;
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            SnapToCurrentSelect();
            _backButton.onClick.AddListener(HandleBack);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _backButton.onClick.RemoveListener(HandleBack);
        }

        private void SnapToCurrentSelect()
        {
            LanguageListElement[] languageListElements = _content.GetComponentsInChildren<LanguageListElement>();
            RectTransform transform = null;
            foreach (var item in languageListElements)
            {
                if (item.Language == _languageChanger.Language)
                {
                    transform = item.transform as RectTransform;
                    break;
                }
            }
            _languageSnapScrol.SnapTo(transform);
        }

        private void HandleBack()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Settings));

        }
    }
}
