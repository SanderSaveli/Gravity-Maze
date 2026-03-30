using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class InputActionManager : MonoBehaviour
    {
        [SerializeField] private LevelTransitionScreenAnimator _transitionScreenAnimator;
        [SerializeField] private BetweenGameAdShower _betweenGameAdShower;
        private SignalBus _signalBus;
        private IGameContext _gameContext;
        private ITimeManager _timeManager;

        [Inject]
        public void Construct(IGameContext gameContext, SignalBus signalBus, ITimeManager timeManager)
        {
            _gameContext = gameContext;
            _signalBus = signalBus;
            _timeManager = timeManager;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalInputAction>(HandleInputAction);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalInputAction>(HandleInputAction);
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
                case InputActionType.RestartGame:
                    await WaitForAdsWithSceneLoad(SceneType.GameScene.ToString());
                    break;
                case InputActionType.LoadNextLevel:
                    await _transitionScreenAnimator.Show(_gameContext.LevelNumber, _gameContext.LevelNumber +1);
                    await WaitForAdsWithSceneLoad(SceneType.GameScene.ToString());
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

        private async UniTask WaitForAdsWithSceneLoad(string sceneName)
        {
            Time.timeScale = 0;

            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
            loadOp.allowSceneActivation = false;

            await _betweenGameAdShower.ShowAdIfNeeded();

            loadOp.allowSceneActivation = true;

            await loadOp.ToUniTask();
            Time.timeScale = _timeManager.CurrentTimeScale;
        }
    }
}