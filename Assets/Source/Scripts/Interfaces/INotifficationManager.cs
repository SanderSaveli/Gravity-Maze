using R3;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public interface INotifficationManager
    {
        public IEnumerable<ColorSheme> UnshownColors { get; }
        public ReactiveProperty<bool> HasUnshownColors { get; }

        public void OnColorShowed(ColorSheme colorSheme);
        public void OnAllColorsShowed();
        public void UnlockNewColor(ColorSheme colorSheme);
    }
}
