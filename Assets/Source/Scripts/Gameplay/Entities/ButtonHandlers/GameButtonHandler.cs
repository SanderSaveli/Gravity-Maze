using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public abstract class GameButtonHandler : MonoBehaviour
    {
        [SerializeField] private GameButton _gameButton;

        private void OnEnable()
        {
            if(_gameButton == null)
            {
                Debug.LogError("Game Button is not assigned");
                return;
            }
            _gameButton.OnActive += HandleGameButtonAction;

        }

        private void OnDisable()
        {
            if (_gameButton == null)
            {
                Debug.LogError("Game Button is not assigned");
                return;
            }
            _gameButton.OnActive -= HandleGameButtonAction;
        }

        protected abstract void HandleGameButtonAction();
    }
}
