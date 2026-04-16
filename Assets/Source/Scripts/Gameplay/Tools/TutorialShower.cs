using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class TutorialShower : MonoBehaviour
    {
        [SerializeField] private TutorialView _view;
        private ITutorialManager _tutorialManager;
        private IGameContext _gameContext;
        private SignalBus _signalBus;
        private bool _isTutorialShowen;

        [Inject]
        public void Construct(SignalBus signalBus, ITutorialManager tutorialManager, IGameContext gameContext)
        {
            _tutorialManager = tutorialManager;
            _gameContext = gameContext;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(OnGameEnd);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(OnGameEnd);
        }

        private void Start()
        {
            if (_tutorialManager.HasTutorialForLevel(_gameContext.LevelNumber + 1, out LevelTutorialData data))
            {
                _isTutorialShowen = true;
                _view.ShowTutorial(data);
            }
        }

        private void OnGameEnd(SignalGameEnd ctx)
        {
            if(ctx.IsWin && _isTutorialShowen)
            {
                _tutorialManager.CompleteTutorial(_gameContext.LevelNumber + 1);
            }
        }
    }
}
