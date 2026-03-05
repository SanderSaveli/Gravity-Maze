using SanderSaveli.UDK.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class RemoveAdsScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _restorePurchaseButton;
        [SerializeField] private Button _buyButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(HandleBack);
            _restorePurchaseButton.onClick.AddListener(HandleRestore);
            _buyButton.onClick.AddListener(HandleBuy);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _backButton.onClick.RemoveListener(HandleBack);
            _restorePurchaseButton.onClick.RemoveListener(HandleRestore);
            _buyButton.onClick.RemoveListener(HandleBuy);
            base.UnsubscribeFromEvents();
        }

        private void HandleBack()
        {
            _signalBus.Fire(new SignalInputOpenMenuScreen(MenuScreenType.Settings));
        }

        private void HandleRestore()
        {

        }

        private void HandleBuy()
        {

        }
    }
}
