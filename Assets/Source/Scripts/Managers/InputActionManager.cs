using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class InputActionManager : MonoBehaviour
    {
        [SerializeField] private LevelTransitionScreenAnimator _transitionScreenAnimator;
        private SignalBus _signalBus;
        private IGameContext _gameContext;

        [Inject]
        public void Construct(IGameContext gameContext)
        {
            _gameContext = gameContext;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalInputAction>(HandleInputAction);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalInputAction>(HandleInputAction);
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private async void HandleInputAction(SignalInputAction input)
        {
            switch (input.Action)
            {
                case InputActionType.ExitGame:
                    ExitGame();
                    break;
                case InputActionType.LoadMenu:
                    SceneManager.LoadScene(SceneType.MenuScene.ToString());
                    break;
                case InputActionType.LoadGame:
                    SceneManager.LoadScene(SceneType.GameScene.ToString());
                    break;
                case InputActionType.LoadNextLevel:
                    await _transitionScreenAnimator.Show(_gameContext.LevelNumber, _gameContext.LevelNumber +1);
                    SceneManager.LoadScene(SceneType.GameScene.ToString());
                    await _transitionScreenAnimator.Hide();
                    break;
                case InputActionType.LoadLevelFromMenu:
                    await _transitionScreenAnimator.Show(_gameContext.LevelNumber+1);
                    SceneManager.LoadScene(SceneType.GameScene.ToString());
                    await _transitionScreenAnimator.Hide();
                    break;
            }
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}