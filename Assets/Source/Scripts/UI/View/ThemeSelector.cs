using SanderSaveli.UDK;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class ThemeSelector : MonoBehaviour
    {
        [SerializeField] private ImageColorSelection _lightView;
        [SerializeField] private ImageColorSelection _darkView;
        [SerializeField] private UIToggleSwitch _toggleSwitch;

        private ThemeType _themeType;

        private void OnEnable()
        {
            _toggleSwitch.OnToggle += OnChangeTheme;
            OnChangeTheme(_toggleSwitch.Value);
        }

        private void OnDisable()
        {
            _toggleSwitch.OnToggle -= OnChangeTheme;
        }

        private void OnChangeTheme(bool isDark)
        {
            if (isDark)
            {
                _darkView.Select();
                _lightView.Deselect();
            }
            else
            {
                _darkView.Deselect();
                _lightView.Select();
            }
        }
    }
}
