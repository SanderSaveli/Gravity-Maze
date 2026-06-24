using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class UnlockAllLevelsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Unlock);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Unlock);
        }

        private void Unlock()
        {
            _levelManager.UnlockAllLevels();
        }
    }
}
