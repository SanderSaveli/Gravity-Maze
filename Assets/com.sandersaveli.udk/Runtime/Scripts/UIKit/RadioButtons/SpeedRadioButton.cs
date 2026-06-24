using CustomText;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class SpeedRadioButton : RadioButton<TimeMode>
    {
        [Header("Components")]
        [SerializeField] private Button _button;
        [SerializeField] private CustomText.CustomText _text;

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
            _text.ChangeColor(_disabledColorStyle);
        }

        public override void Select()
        {
            _text.ChangeColor(_enabledColorStyle);
        }
    }
}
