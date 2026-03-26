using R3;

namespace SanderSaveli.GravityMaze
{
    public interface IAppSettings
    {
        public ReactiveProperty<bool> IsMusicOn { get; }
        public ReactiveProperty<bool> IsSoundOn { get; }
        public ReactiveProperty<bool> IsVibrationOn { get; }
        public ReactiveProperty<Language> Language { get; }
        public ReactiveProperty<ColorSheme> ColorSheme { get; }
        public ReactiveProperty<TimeMode> TimeMode { get; }
    }
}
