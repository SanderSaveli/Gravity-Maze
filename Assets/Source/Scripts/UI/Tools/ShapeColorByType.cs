using CustomText;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(SpriteShapeRenderer))]
    public class ShapeColorByType : MonoBehaviour
    {
        public Custom_ColorStyle Color => _type;

        [SerializeField] private Custom_ColorStyle _type;
        [SerializeField] private SpriteShapeRenderer _image;

        private bool _isSubcribed = false;
        private Custom_ColorStyle _selectedColor = Custom_ColorStyle.Default;


        private void Awake() => ApplyColorSetting();

        private void ApplyColorSetting()
        {
            if(_image == null)
            {
                OnDestroy();
                return;
            }
            _selectedColor = _type;
            if (ColorSettings.Instance == null) return;
            Color color = ColorSettings.Instance.GetColorByStyle(_selectedColor);
            _image.color = color;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Change();
        }
#endif

        private void Change()
        {
            _image = GetComponent<SpriteShapeRenderer>();
            if (_image == null)
            {
                Debug.LogError("Image not found in " + gameObject.name);
                return;
            }

            if (!_isSubcribed)
            {
                ColorSettings.Instance.OnColorStyleChanged += ApplyColorSetting;
                _isSubcribed = true;
            }

            if (_type != _selectedColor) ApplyColorSetting();
        }

        private void OnDestroy()
        {
            ColorSettings.Instance.OnColorStyleChanged -= ApplyColorSetting;
            DOTween.Kill(_image);
        }

        public void ChangeColor(Custom_ColorStyle style)
        {
            _selectedColor = style;
            if (ColorSettings.Instance == null) return;

            _type = _selectedColor;
            Change();
        }
    }
}
