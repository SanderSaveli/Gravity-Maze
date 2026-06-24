using CustomText;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorManager : MonoBehaviour, IColorManager
    {
        public ReactiveProperty<ColorSheme> ActiveSheme { get; private set; }

        public IReadOnlyList<ColorOverrides> ColorOverrides => _activeeShemeSO.Overrides;
        public IReadOnlyList<ColorContext> ColorContexts => 
            _lightColors
            .Concat(_darkColors)
            .Concat(_multicolorColors)
            .ToList();

        public IReadOnlyList<ColorContext> LightColors => _lightColors;

        public IReadOnlyList<ColorContext> DarkColors => _darkColors;

        public IReadOnlyList<ColorContext> MulticolorColors => _multicolorColors;

        [Header("ColorParams")]
        [SerializeField] private List<ShemeColorPairs> _shemeColors;

        [Header("Light Colors")]
        [SerializeField] private List<ColorContext> _lightColors;
        [Header("Dark Colors")]
        [SerializeField] private List<ColorContext> _darkColors;
        [Header("Dark Colors")]
        [SerializeField] private List<ColorContext> _multicolorColors;

        private ColorSetSO _activeeShemeSO;
        private ColorSettings _colorSettings;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            ActiveSheme = appSettings.ColorSheme;
        }

        private void OnEnable()
        {
            _colorSettings = ColorSettings.Instance;
            _compositeDisposable = new CompositeDisposable();
            ActiveSheme.Subscribe(ChangeColor).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void ChangeColor(ColorSheme color)
        {
            Debug.Log("Colors Changed: " + color);
            ShemeColorPairs pair = GetPair(color);
            if (pair == null)
            {
                Debug.LogError($"There is no sheme for color {color}");
                return;
            }
            _activeeShemeSO = pair.ColorSet;
            _colorSettings.SetNewColors(_activeeShemeSO.Colors);
        }

        public void PreviewSheme(ColorSheme color)
        {
            ShemeColorPairs pair = GetPair(color);
            if (pair == null)
            {
                Debug.LogError($"There is no sheme for color {color}");
                return;
            }
            _colorSettings.SetNewColors(pair.ColorSet.Colors);
        }

        private ShemeColorPairs GetPair(ColorSheme color) => _shemeColors.FirstOrDefault(t => t.ColorSheme == color);

        public void ShowActiveSheme()
        {
            ChangeColor(ActiveSheme.Value);
        }

        public Color GetActiveColorOfSheme(ColorSheme sheme)
        {
            Debug.Log(sheme.ToString());
            ShemeColorPairs pair = GetPair(sheme);
            if (pair == null)
            {
                Debug.LogError($"There is no sheme for color {sheme}");
                return Color.white;
            }
            return pair.ColorSet.Colors.Find(t => t.TextColorType == Custom_ColorStyle.Default).Color;
        }

        [Serializable]
        private class ShemeColorPairs
        {
            public ColorSheme ColorSheme;
            public ColorSetSO ColorSet;
        }
    }
}
