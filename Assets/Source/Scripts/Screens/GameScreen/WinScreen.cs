using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class WinScreen : UiScreen
    {
        [Header("Buttons")]
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _exitToMenu;

        private SignalBus _signalBus;
        private IGameContext _gameContext;
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(SignalBus signalBus, IGameContext gameContext, ILevelManager levelManager)
        {
            _signalBus = signalBus;
            _gameContext = gameContext;
            _levelManager = levelManager;
        }

        protected override void SubscribeToEvents()
        {
            _nextButton.onClick.AddListener(HandleNext);
            _exitToMenu.onClick.AddListener(HandleExitToMenu);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _nextButton.onClick.RemoveListener(HandleNext);
            _exitToMenu.onClick.RemoveListener(HandleExitToMenu);
            base.UnsubscribeFromEvents();
        }

        private void HandleNext()
        {
            int level = _gameContext.LevelNumber;
            level = Mathf.Clamp(level + 1, 0, _levelManager.Levels.Count-1);
            _gameContext.LevelNumber = level;
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadNextLevel));
        }

        private void HandleExitToMenu()
        {
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadMenu));
        }
    }
}
