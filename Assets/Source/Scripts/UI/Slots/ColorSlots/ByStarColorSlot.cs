using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ByStarColorSlot : ClosableColorSlot
    {
        private ILevelStorage _levelStorage;

        [Inject]
        public void Construct(ILevelStorage levelStorage)
        {
            _levelStorage = levelStorage;
        }

        protected override bool CanSelect()
        {
            if (_levelStorage.StarCount >= ColorContext.StarToUnlock)
            {
                return true;
            }
            else
            {
                OpenPreview();
                return false;
            }
        }

        protected override bool IsOpened() => _levelStorage.StarCount >= ColorContext.StarToUnlock;
    }
}
