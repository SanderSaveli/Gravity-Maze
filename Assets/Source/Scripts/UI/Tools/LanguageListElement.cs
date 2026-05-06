using SanderSaveli.UDK;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LanguageListElement : MonoBehaviour, ISelectable
    {
        public Language Language => _language;
        public bool IsSelected => _selected;

        [SerializeField] private Language _language;
        [SerializeField] private Button _button;
        [SerializeField] private SelectingSnapScroll _scroll;
        [SerializeField] private RectTransform _selfTransform;

        private bool _selected;
        private ILanguageChanger<Language> _languageChanger;
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(ILanguageChanger<Language> languageChanger, IAppSettings appSettings)
        {
            _languageChanger = languageChanger;
            _appSettings = appSettings;
        }

        private void Reset()
        {
            _selfTransform = gameObject.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
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

        private void HandleClick()
        {
            _scroll.SnapTo(_selfTransform);
        }
    }
}
