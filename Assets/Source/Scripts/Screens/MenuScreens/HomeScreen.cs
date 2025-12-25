using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class HomeScreen : UiScreen
    {
        [SerializeField] private Button _playButton;
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
            _playButton.onClick.AddListener(HandlePlay);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _playButton.onClick.RemoveListener(HandlePlay);
            base.UnsubscribeFromEvents();
        }

        private void HandlePlay()
        {
            _gameContext.LevelNumber = _levelManager.CurrentLevel;
            _signalBus.Fire(new SignalInputAction(InputActionType.LoadGame));
        }
    }
}
