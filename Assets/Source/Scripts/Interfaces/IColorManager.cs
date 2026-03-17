using R3;

namespace SanderSaveli.GravityMaze
{
    public interface IColorManager
    {
        public ReactiveProperty<ColorSheme> ActiveSheme { get; }
    }
}
