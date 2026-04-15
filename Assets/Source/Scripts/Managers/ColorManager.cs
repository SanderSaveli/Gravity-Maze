using CustomText;
using R3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorManager : MonoBehaviour, IColorManager
    {
        public ReactiveProperty<ColorSheme> ActiveSheme { get; private set; }

        public IReadOnlyList<ColorOverrides> ColorOverrides => _activeeShemeSO.Overrides;

        [SerializeField] private List<ShemeColorPairs> _shemeColors;
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
            ShemeColorPairs pair = _shemeColors.FirstOrDefault(t => t.ColorSheme == color);
            if (pair == null)
            {
                Debug.LogError($"There is no sheme for color {color}");
                return;
            }
            _activeeShemeSO = pair.ColorSet;
            _colorSettings.SetNewColors(_activeeShemeSO.Colors);
        }

        [Serializable]
        private class ShemeColorPairs
        {
            public ColorSheme ColorSheme;
            public ColorSetSO ColorSet;
        }
    }
}
