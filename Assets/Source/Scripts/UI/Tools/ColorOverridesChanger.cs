using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorOverridesChanger : MonoBehaviour
    {
        [SerializeField] private Image _color;
        [SerializeField] private Image _select;
        [SerializeField] private ColorRadioButton _colorRadioButton;
        private Color _originalColor;
        private IColorManager _colorManager;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public void Construct(IColorManager colorManager)
        {
            _colorManager = colorManager;
        }

        private void Awake()
        {
            _originalColor = _color.color;
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _colorManager.ActiveSheme.Subscribe(ApplyColorSetting).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void ApplyColorSetting(ColorSheme sheme)
        {
            IReadOnlyList<ColorOverrides> overrides = _colorManager.ColorOverrides;
            Color color = _originalColor;

            if(overrides != null)
            {
                if (overrides.Select(t => t.Theme).Contains(_colorRadioButton.Value))
                {
                    color = overrides.ToList().Find(t => t.Theme == _colorRadioButton.Value).Override;
                }
            }

            _color.color = color;
            color.a = _select.color.a;
            _select.color = color;
        }
    }
}
