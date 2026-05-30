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
        private Tween _tween;

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
            KillColorTween();
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
            KillColorTween();
        }

        public void ChangeColor(Custom_ColorStyle style)
        {
            _selectedColor = style;
            if (ColorSettings.Instance == null) return;
            
            _type = _selectedColor;
            KillColorTween();
            Change();
        }

        public void ChangeColorWithAnimation(Custom_ColorStyle style, float duration = 0.4f)
        {
            if (ColorSettings.Instance == null) return;

            _selectedColor = style;
            _type = style;

            KillColorTween();

            Color targetColor = ColorSettings.Instance.GetColorByStyle(style);

            if (!_isOverrideAlpha)
                targetColor.a = _image.color.a;

            _tween = _image
                .DOColor(targetColor, duration)
                .OnComplete(() => _tween = null)
                .SetLink(_image.gameObject);
        }

        private void KillColorTween()
        {
            if (_image != null)
                _image.DOKill();

            _tween?.Kill();
            _tween = null;
        }
    }
}
