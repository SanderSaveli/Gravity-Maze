using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorChangeSelectedElement : MonoBehaviour, ISelectable
    {
        [SerializeField] private Image _imgae;
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _deselectedColor = Color.gray;
        [Min(0)]
        [SerializeField] private float _animationDuration = 0.1f;
        public bool IsSelected { get; set; }

        private void Start()
        {
            if(IsSelected)
            {
                _imgae.color = _selectedColor;
            }
            else
            {
                _imgae.color = _deselectedColor;
            }
        }

        public void Deselect()
        {
            IsSelected = false;
            _imgae.DOColor(_deselectedColor, _animationDuration);
        }

        public void Select()
        {
            IsSelected = true;
            _imgae.DOColor(_selectedColor, _animationDuration);
        }
    }
}
