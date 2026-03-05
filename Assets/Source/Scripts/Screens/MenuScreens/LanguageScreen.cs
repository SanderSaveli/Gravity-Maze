using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LanguageScreen : UiScreen
    {
        [SerializeField] private SelectingSnapScroll _languageSnapScrol;
        [SerializeField] private Transform _content;
        private ILanguageChanger<Language> _languageChanger;

        [Inject]
        public void Construct(ILanguageChanger<Language> language)
        {
            _languageChanger = language;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            SnapToCurrentSelect();
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
    }
}
