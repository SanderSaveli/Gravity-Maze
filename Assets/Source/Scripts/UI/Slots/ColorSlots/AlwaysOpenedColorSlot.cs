namespace SanderSaveli.GravityMaze
{
    public class AlwaysOpenedColorSlot : ColorSlot
    {
        protected override bool CanSelect() => true;
    }
}
