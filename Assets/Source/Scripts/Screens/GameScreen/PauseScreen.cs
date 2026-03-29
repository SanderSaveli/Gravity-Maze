using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class PauseScreen : PauseUIScreen
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _restartButto;
        [SerializeField] private Button _exitButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void SubscribeToEvents()
        {
            _continueButton.onClick.AddListener(HandleResume);
            _restartButto.onClick.AddListener(HandleRestart);
            _exitButton.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _continueButton.onClick.RemoveListener(HandleResume);
            _restartButto.onClick.RemoveListener(HandleRestart);
            _exitButton.onClick.RemoveListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleResume()
        {
            Hide();
        }

        private void HandleExitToMenu()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.ExitGame));
        }

        private void HandleRestart()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.RestartGame));
        }
    }
}
