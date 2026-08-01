using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AboutUsScreen : UiScreen
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _rateButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            _closeButton.onClick.AddListener(HandleClose);
            _privacyButton.onClick.AddListener(HandlePrivacy);
            _rateButton.onClick.AddListener(HandleRate);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _closeButton.onClick.RemoveListener(HandleClose);
            _privacyButton.onClick.RemoveListener(HandlePrivacy);
            _rateButton.onClick.RemoveListener(HandleRate);
            base.UnsubscribeFromEvents();
        }

        private void HandleClose()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Settings));
        }

        private void HandlePrivacy()
        {
            Application.OpenURL(Const.PrivacyPolicyURL);
        }

        private void HandleRate()
        {
            Application.OpenURL(Const.ShopURL);
        }
    }
}
