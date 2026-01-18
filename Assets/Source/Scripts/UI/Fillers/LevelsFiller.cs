using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SanderSaveli.GravityMaze
{
    public class LevelsFiller : SlotFiller<LevelSlot, LevelData>, ISelectable
    {
        public Action<LevelsFiller> OnLevelListSelected { get; set; }
        public IReadOnlyList<LevelSlot> Levels => _slots;
        public int MinLevel { get; private set; }
        public int MaxLevel { get; private set; }

        public bool IsSelected { get; private set; }

        public override void FillItems(List<LevelData> items)
        {
            base.FillItems(items);
            MinLevel = items.First().Number;
            MaxLevel = items.Last().Number;
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        public void Select()
        {
            OnLevelListSelected?.Invoke(this);
            IsSelected = true;
        }
    }
}
