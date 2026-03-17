using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class AlwaysOpenedColorSlot : ColorRadioButton
    {
        protected override bool CanSelect() => true;
    }
}
