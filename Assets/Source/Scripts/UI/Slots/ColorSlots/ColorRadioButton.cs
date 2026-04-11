using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public abstract class ColorRadioButton : RadioButton<ColorSheme>
    {
        [SerializeField] protected Button _button;

        protected void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        protected void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        public override void Deselect()
        { }

        public override void Select()
        { }

        private void HandleClick()
        {
            if (CanSelect())
            {
                OnSelectInput?.Invoke(this);
            }
        }

        protected abstract bool CanSelect();
    }
}
