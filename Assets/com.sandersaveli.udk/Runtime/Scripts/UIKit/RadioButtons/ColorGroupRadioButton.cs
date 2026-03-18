using CustomText;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroupRadioButton : RadioButton<ColorGroupType>
    {
        [Header("Components")]
        [SerializeField] private Button _button;
        [SerializeField] private ImageColorByType _iconImage;

        [Header("Params")]
        [SerializeField] private float _animationDuration = 0.4f;
        [SerializeField] private Custom_ColorStyle _enabledColorStyle;
        [SerializeField] private Custom_ColorStyle _disabledColorStyle;

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        private void HandleClick()
        {
            OnSelectInput.Invoke(this);    
        }

        public override void Deselect()
        {
            _iconImage.ChangeColorWithAnimation(_disabledColorStyle, _animationDuration);
        }

        public override void Select()
        {
            _iconImage.ChangeColorWithAnimation(_enabledColorStyle, _animationDuration);
        }
    }
}
