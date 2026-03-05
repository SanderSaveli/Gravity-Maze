using CustomText;
using DG.Tweening;
using SanderSaveli.UDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class SettingsSwichableButton : SelectButton
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private TextByTableKey _textByTableKey;

        [Header("Properties")]
        [SerializeField] private float _animationDuration = 0.5f;
        [Space]
        [SerializeField] private Custom_ColorStyle _textSelectColor;
        [SerializeField] private Custom_ColorStyle _testDeselectColor;
        [Space]
        [SerializeField] private Custom_ColorStyle _imageSelectColor;
        [SerializeField] private Custom_ColorStyle _imageDeselectColor;
        [Space]
        [SerializeField] private string _selectedTextKey;
        [SerializeField] private string _deselectTextKey;
        private ColorSettings _colorSettings;

        [Inject]
        public void Construct()
        {
            _colorSettings = ColorSettings.Instance;
        }

        protected override void HandleDeselect()
        {
            _text.text = "off";
            Color textColor = _colorSettings.GetColorByStyle(_testDeselectColor);
            _text.DOColor(textColor, _animationDuration).SetLink(gameObject);

            Color imageColor = _colorSettings.GetColorByStyle(_imageDeselectColor);
            _image.DOColor(imageColor, _animationDuration).SetLink(gameObject);

            _textByTableKey.ChangeText(_deselectTextKey);
        }

        protected override void HandleSelect()
        {
            _text.text = "on";
            Color textColor = _colorSettings.GetColorByStyle(_textSelectColor);
            _text.DOColor(textColor, _animationDuration).SetLink(gameObject);

            Color imageColor = _colorSettings.GetColorByStyle(_imageSelectColor);
            _image.DOColor(imageColor, _animationDuration).SetLink(gameObject);

            _textByTableKey.ChangeText(_selectedTextKey);
        }
    }
}
