using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ClosableUIScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private MenuScreenType _afterCloseScreen;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _backButton.onClick.AddListener(HandleBack);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _backButton.onClick.RemoveListener(HandleBack);
        }

        protected void HandleBack()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(_afterCloseScreen));
        }
    }
}
