using SanderSaveli.UDK.UI;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class HomeScreen : UiScreen
    {
        [SerializeField] private NavBarRadioGroup _navBarRadioGroup;

        protected override void SubscribeToEvents()
        {
            _navBarRadioGroup.OnValueChanged += HavBarValueChange;
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _navBarRadioGroup.OnValueChanged -= HavBarValueChange;
            base.UnsubscribeFromEvents();
        }

        private void HavBarValueChange(NavBarOption option)
        {
            Debug.Log(option.ToString());
            if (option == NavBarOption.Home)
            {
                Debug.Log("Show");
                Show();
            }
            else
            {
                Debug.Log("Hide");
                Hide();
            }
        }
    }
}
