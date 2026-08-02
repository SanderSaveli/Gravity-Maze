using SanderSaveli.UDK.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class SupportUsScreen : UiScreen
    {
        [SerializeField] private SupportUsShower _shower;
        [SerializeField] private StarAnimator _animator;
        [SerializeField] private Button _rateButton;
        [SerializeField] private Button _maybeLaterButton;
        [SerializeField] private Button _neverButton;
        private IAnalyticManager _analyticManager;

        [Inject]
        public void Construct(IAnalyticManager analyticManager)
        {
            _analyticManager = analyticManager;
        }

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

        public override void Show(Action callback = null)
        {
            _analyticManager.SendSupportUsScreenShow();
            _ = _animator.AnimateStars();
            base.Show(callback);
        }

        private void HandleRate()
        {
            _analyticManager.SendSupportUsScreenSupportButtonClicked();
            _shower.ShowComplete();
            Hide();
            Application.OpenURL(Const.ShopURL);
        }

        private void HandleMaybeLater()
        {
            _analyticManager.SendSupportUsScreenMaybeLaterButtonClicked();

            _shower.MaybeLater();
            Hide();
        }

        private void HandleHever()
        {
            _analyticManager.SendSupportUsScreenNeverButtonClicked();

            _shower.ShowComplete();
            Hide();
        }
    }
}
