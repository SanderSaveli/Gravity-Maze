using Cysharp.Threading.Tasks;
using SanderSaveli.UDK;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LanguageScreen : ClosableUIScreen
    {
        [SerializeField] private SelectingSnapScroll _languageSnapScrol;
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
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
        }

        private async void SnapToCurrentSelect()
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
            await UniTask.Yield();
            _languageSnapScrol.SnapImmediatley(transform);
        }
    }
}
