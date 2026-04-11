using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ByStarColorSlot : ClosableColorSlot
    {
        [Header("Components")]
        [SerializeField] private ColorByStarScreen _colorByStarScreen;

        [Header("Params")]
        [SerializeField] private int _needStarsToUnlock;

        private ILevelStorage _levelStorage;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(ILevelStorage levelStorage, SignalBus signalBus)
        {
            _levelStorage = levelStorage;
            _signalBus = signalBus;
        }

        protected override bool CanSelect()
        {
            if (_levelStorage.StarCount >= _needStarsToUnlock)
            {
                return true;
            }
            else
            {
                OpenPreview();
                return false;
            }
        }

        protected override void OpenPreview()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.ColorByStars));
            _colorByStarScreen.Init(_needStarsToUnlock);
        }

        protected override bool IsOpened() => _levelStorage.StarCount >= _needStarsToUnlock;
    }
}
