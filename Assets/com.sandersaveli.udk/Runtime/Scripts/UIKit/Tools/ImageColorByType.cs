using CustomText;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.UDK.UI
{
    [AddComponentMenu("UI/Custom Components/Image Color By Type")]
    [RequireComponent(typeof(Image))]
    public class ImageColorByType : MonoBehaviour
    {
        public Custom_ColorStyle Color => _type;

        [SerializeField] private Custom_ColorStyle _type;
        [SerializeField] private Image _image;
        [SerializeField] private bool _isOverrideAlpha = true;

        private bool _isSubcribed = false;
        private Custom_ColorStyle _selectedColor = Custom_ColorStyle.Default;


        private void OnEnable()
        {
            ColorSettings.Instance.OnColorStyleChanged += ApplyColorSetting;
            ApplyColorSetting();
        }

        private void OnDisable()
        {
            ColorSettings.Instance.OnColorStyleChanged -= ApplyColorSetting;
        }

        private void Start()
        {
            Change();
        }

        private void ApplyColorSetting()
        {
            _selectedColor = _type;
            Color color = ColorSettings.Instance.GetColorByStyle(_selectedColor);
            if (!_isOverrideAlpha)
            {
                color.a = _image.color.a;
            }
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
            _image = GetComponent<Image>();
            if (_image == null)
            {
                Debug.LogError("Image not found in " + gameObject.name);
                return;
            }

            if (!_isSubcribed)
            {
                _isSubcribed = true;
            }

            ApplyColorSetting();
        }

        private void OnDestroy()
        {
            DOTween.Kill(_image);
        }

        public void ChangeColor(Custom_ColorStyle style)
        {
            _selectedColor = style;
            if (ColorSettings.Instance == null) return;
            
            _type = _selectedColor;
            Change();
        }

        public void ChangeColorWithAnimation(Custom_ColorStyle style, float duration = 0.4f)
        {
            _selectedColor = style;

            if (ColorSettings.Instance == null) return;

            Color color = ColorSettings.Instance.GetColorByStyle(style);
            _type = _selectedColor;
            Change();
            _image.DOColor(color, duration);
        }
    }
}