using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class SupportUsScreen : UiScreen
    {
        [SerializeField] private SupportUsShower _shower;
        [SerializeField] private Button _rateButton;
        [SerializeField] private Button _maybeLaterButton;
        [SerializeField] private Button _neverButton;

        protected override void SubscribeToEvents()
        {
            _rateButton.onClick.AddListener(HandleRate);
            _maybeLaterButton.onClick.AddListener(HandleMaybeLater);
            _neverButton.onClick.AddListener(HandleHever);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _rateButton.onClick.RemoveListener(HandleRate);
            _maybeLaterButton.onClick.RemoveListener(HandleMaybeLater);
            _neverButton.onClick.RemoveListener(HandleHever);
            base.UnsubscribeFromEvents();
        }

        private void HandleRate()
        {
            _shower.ShowComplete();
            Hide();
            Application.OpenURL(Const.ShopURL);
        }

        private void HandleMaybeLater()
        {
            _shower.MaybeLater();
            Hide();
        }

        private void HandleHever()
        {
            _shower.ShowComplete();
            Hide();
        }
    }
}
