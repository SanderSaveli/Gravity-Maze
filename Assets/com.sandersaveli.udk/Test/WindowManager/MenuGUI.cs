using SanderSaveli.UDK.UI;
using UnityEngine;

namespace SanderSaveli.UDK
{
    public class MenuGUI : MonoBehaviour
    {
        [SerializeField] private MainMenuScreenManager _screenManager;
        public void OpenMenu()
        {
            _screenManager.OpenScreen(MenuScreenType.Home);
        }

        public void OpenSettings()
        {
            _screenManager.OpenScreen(MenuScreenType.Settings);
        }

        public void OpenFAQ()
        {
            _screenManager.OpenScreen(MenuScreenType.Levels);
        }
    }
}
