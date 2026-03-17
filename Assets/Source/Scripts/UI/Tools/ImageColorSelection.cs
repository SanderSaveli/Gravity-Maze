using CustomText;
using DG.Tweening;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class ImageColorSelection : MonoBehaviour, ISelectable
    {
        [SerializeField] private Image _image;
        [SerializeField] private ImageColorByType _imageColorByType;
        [Space]
        [SerializeField] private Custom_ColorStyle _enableColor;
        [SerializeField] private Custom_ColorStyle _disableColor;
        [SerializeField] private float _animationDuration = 0.5f;
        public bool IsSelected { get; private set; }
        private ColorSettings _colorSettings;

        private void Awake()
        {
            _colorSettings = ColorSettings.Instance;
        }

        public void Deselect()
        {
            IsSelected = false;
            Color color = _colorSettings.GetColorByStyle(_disableColor);
            _image.DOColor(color, _animationDuration)
                .OnComplete(() => _imageColorByType.ChangeColor(_disableColor));
        }

        public void Select()
        {
            IsSelected = true;
            Color color = _colorSettings.GetColorByStyle(_enableColor);
            _image.DOColor(color, _animationDuration)
                .OnComplete(() => _imageColorByType.ChangeColor(_enableColor));
        }
    }
}
