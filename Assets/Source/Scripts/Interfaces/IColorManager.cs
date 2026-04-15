using R3;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public interface IColorManager
    {
        public ReactiveProperty<ColorSheme> ActiveSheme { get; }

        public IReadOnlyList<ColorOverrides> ColorOverrides { get; }
    }
}
