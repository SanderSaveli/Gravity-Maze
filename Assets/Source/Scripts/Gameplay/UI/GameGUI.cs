using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameGUI : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _homeButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(HandleRestart);
            _homeButton.onClick.AddListener(HandleHome);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(HandleRestart);
            _homeButton.onClick.RemoveListener(HandleHome);
        }

        private void HandleRestart()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.RestartGame));
        }

        private void HandleHome()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
        }
    }
}
