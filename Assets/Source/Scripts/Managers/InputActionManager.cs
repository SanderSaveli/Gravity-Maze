using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class InputActionManager : MonoBehaviour
    {
        private SignalBus _signalBus;

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

        private void HandleInputAction(SignalInputAction input)
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
            }
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}