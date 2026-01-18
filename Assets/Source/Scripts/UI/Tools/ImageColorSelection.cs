using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ImageColorSelection : MonoBehaviour, ISelectable
    {
        [SerializeField] private Image _image;
        [Space]
        [SerializeField] private Color _enableColor = Color.white;
        [SerializeField] private Color _disableColor = Color.gray;
        [SerializeField] private float _animationDuration = 0.5f;
        public bool IsSelected { get; private set; }

        public void Deselect()
        {
            IsSelected = false;
            _image.DOColor(_disableColor, _animationDuration);
        }

        public void Select()
        {
            IsSelected = true;
            _image.DOColor(_enableColor, _animationDuration);
        }
    }
}
