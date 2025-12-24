using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameEndHanler : MonoBehaviour
    {
        [SerializeField] private UiScreen _winScreen;

        private SignalBus _signalBus;
        private IInputManager _inputManager;
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(SignalBus signalBus, IInputManager inputManager, ILevelManager levelManager)
        {
            _signalBus = signalBus;
            _inputManager = inputManager;
            _levelManager = levelManager;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(HandleGameEnd);
        }

        private void HandleGameEnd(SignalGameEnd gameEnd)
        {
            _inputManager.IsEnabled = false;

            _winScreen.Show();
            _levelManager.CompleteLevel(_levelManager.CurrentLevel + 1);
        }
    }
}
