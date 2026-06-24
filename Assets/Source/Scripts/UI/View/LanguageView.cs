using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LanguageView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private IAppSettings _appSettings;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _appSettings.Language.Subscribe(UpdateLaguageView).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void UpdateLaguageView(Language language)
        {
            _text.text = language.ToString();
        }
    }
}
