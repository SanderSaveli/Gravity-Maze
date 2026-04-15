using DG.Tweening;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public abstract class ColorRadioButton : RadioButton<ColorSheme>
    {
        [SerializeField] protected Button _button;
        [SerializeField] protected Transform _select;

        [Header("Params")]
        [SerializeField] private float _selectDuration = 0.5f;
        [SerializeField] private float _selectScale = 1.2f;

        protected void OnEnable()
        {
            _button.onClick.AddListener(HandleClick);
        }

        protected void OnDisable()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        public override void Deselect()
        { 
            _select.DOScale(1, _selectDuration).SetLink(gameObject);
        }

        public override void Select()
        {
            _select.DOScale(_selectScale, _selectDuration).SetLink(gameObject);
        }

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
