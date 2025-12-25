using SanderSaveli.UDK.UI;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public class LevelsFiller : SlotFiller<LevelSlot, LevelData>
    {
        public IReadOnlyList<LevelSlot> Levels => _slots;
    }
}
