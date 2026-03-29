using CustomText;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailColorByType : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private Custom_ColorStyle _type;
        public Custom_ColorStyle Color => _type;

        private bool _isSubcribed = false;
        private Custom_ColorStyle _selectedColor = Custom_ColorStyle.Default;


        private void Reset()
        {
            _trailRenderer = gameObject.GetComponent<TrailRenderer>();
        }

        private void Awake() => ApplyColorSetting();

        private void ApplyColorSetting()
        {
            if (_trailRenderer == null)
            {
                OnDestroy();
                return;
            }

            _selectedColor = _type;
            Color color = ColorSettings.Instance.GetColorByStyle(_selectedColor);
            Gradient gradient = _trailRenderer.colorGradient;

            var colorKeys = gradient.colorKeys;

            for (int i = 0; i < colorKeys.Length; i++)
            {
                Color c = color;
                c.a = colorKeys[i].color.a;
                colorKeys[i].color = c;
            }

            gradient.colorKeys = colorKeys;
            _trailRenderer.colorGradient = gradient;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Change();
        }
#endif

        private void Change()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
            if (_trailRenderer == null)
            {
                Debug.LogError("Renderrer not found in " + gameObject.name);
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
            DOTween.Kill(_trailRenderer);
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
