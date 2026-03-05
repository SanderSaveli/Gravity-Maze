using SanderSaveli.UDK;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LanguageListElement : MonoBehaviour, ISelectable
    {
        public Language Language => _language;
        public bool IsSelected => _selected;

        [SerializeField] private Language _language;
        private bool _selected;
        private ILanguageChanger<Language> _languageChanger;
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(ILanguageChanger<Language> languageChanger, IAppSettings appSettings)
        {
            _languageChanger = languageChanger;
            _appSettings = appSettings;
        }

        public void Deselect()
        {
            _selected = false;
        }

        public void Select()
        {
            _selected = true;
            _languageChanger.ChangeLanguage(_language);
            _appSettings.Language.Value = _language;
        }
    }
}
