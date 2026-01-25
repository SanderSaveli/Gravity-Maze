using CustomText;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class SettingsSwichableButton : SelectButton
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;

        [Header("Properties")]
        [SerializeField] private float _animationDuration = 0.5f;
        [Space]
        [SerializeField] private Custom_ColorStyle _textSelectColor;
        [SerializeField] private Custom_ColorStyle _testDeselectColor;
        [Space]
        [SerializeField] private Custom_ColorStyle _imageSelectColor;
        [SerializeField] private Custom_ColorStyle _imageDeselectColor;
        private ColorSettings _colorSettings;

        private void Awake()
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
        }

        protected override void HandleSelect()
        {
            _text.text = "on";
            Color textColor = _colorSettings.GetColorByStyle(_textSelectColor);
            _text.DOColor(textColor, _animationDuration).SetLink(gameObject);

            Color imageColor = _colorSettings.GetColorByStyle(_imageSelectColor);
            _image.DOColor(imageColor, _animationDuration).SetLink(gameObject);
        }
    }
}
