using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroupRadioButton : RadioButton<ColorGroupType>
    {
        [SerializeField] private Button _button;

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

        }

        public override void Select()
        {

        }
    }
}
