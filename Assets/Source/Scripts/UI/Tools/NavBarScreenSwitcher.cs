using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class NavBarScreenSwitcher : MonoBehaviour
    {
        [SerializeField] private NavBarRadioGroup _navBar;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _navBar.OnValueChanged += SwitchScreen;
            SwitchScreen(_navBar.Value);
        }

        private void OnDisable()
        {
            _navBar.OnValueChanged -= SwitchScreen;
        }

        private void SwitchScreen(NavBarOption option)
        {
            switch (option)
            {
                case NavBarOption.Home:
                    _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Home));
                    break;
                case NavBarOption.Levels:
                    _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Levels));
                    break;
                case NavBarOption.Color:
                    _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Color));
                    break;
                case NavBarOption.Settings:
                    _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Settings));
                    break;
                default:
                    throw new System.NotImplementedException($"There is no case for {nameof(option)} = {option}");
            }
        }
    }
}
