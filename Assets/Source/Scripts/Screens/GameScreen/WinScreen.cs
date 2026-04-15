using DG.Tweening;
using SanderSaveli.UDK.UI;
using System;
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
        [Header("Params")]
        [SerializeField] private float _rotatablePartScale = 0.85f;
        [SerializeField] private float _duration = 0.5f;

        private SignalBus _signalBus;
        private IGameContext _gameContext;
        private ILevelManager _levelManager;
        private ILevelProvider _levelProvider;

        [Inject]
        public void Construct(SignalBus signalBus, IGameContext gameContext, ILevelManager levelManager, ILevelProvider levelProvider)
        {
            _signalBus = signalBus;
            _gameContext = gameContext;
            _levelManager = levelManager;
            _levelProvider = levelProvider;
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

        public override void Show(Action callback = null)
        {
            base.Show(callback);
            _levelProvider.RotablePart.DOScale(_rotatablePartScale, _duration);
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
