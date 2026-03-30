using CustomText;
using R3;
using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class RemoveAdsButton : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private ImageColorByType _imageColor;
        [SerializeField] private CustomText.CustomText _text;
        [SerializeField] private TextByTableKey _textByTableKey;

        [Header("Preperties")]
        [Header("Sprite")]
        [SerializeField] private Sprite _enabledSprite;
        [SerializeField] private Sprite _disabledSprite;
        [Header("Color")]
        [SerializeField] private Custom_ColorStyle _enabledColor;
        [SerializeField] private Custom_ColorStyle _disabledColor;
        [Header("Text")]
        [SerializeField] private string _enabledTextKey;
        [SerializeField] private string _disabledTextKey;

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
            _appSettings.IsAdsRemoved.Subscribe(ChangeAdStatus).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void ChangeAdStatus(bool isAdsRemoved)
        {
            _button.interactable = !isAdsRemoved;

            _icon.sprite = isAdsRemoved ? _enabledSprite : _disabledSprite;
            _textByTableKey.ChangeText(isAdsRemoved ? _enabledTextKey : _disabledTextKey);
            _text.ChangeColor(isAdsRemoved ? _enabledColor : _disabledColor);
            _imageColor.ChangeColor(isAdsRemoved ? _enabledColor : _disabledColor);
        }
    }
}
