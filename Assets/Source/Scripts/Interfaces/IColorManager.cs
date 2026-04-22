using R3;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public interface IColorManager
    {
        public ReactiveProperty<ColorSheme> ActiveSheme { get; }

        public IReadOnlyList<ColorOverrides> ColorOverrides { get; }
        public IReadOnlyList<ColorContext> LightColors { get; }
        public IReadOnlyList<ColorContext> DarkColors { get; }
        public IReadOnlyList<ColorContext> MulticolorColors { get; }

        public Color GetActiveColorOfSheme(ColorSheme sheme);

        public void PreviewSheme(ColorSheme color);

        public void ShowActiveSheme();
    }
}
