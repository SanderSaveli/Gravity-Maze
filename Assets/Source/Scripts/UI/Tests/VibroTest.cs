using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class VibroTest : MonoBehaviour
    {
        [SerializeField] private Button _smallButton;
        [SerializeField] private Button _mediumButton;
        [SerializeField] private Button _largeButton;
        private IVibrationManager _vibrationManager;

        [Inject]
        public void Construct(IVibrationManager vibrationManager)
        {
            _vibrationManager = vibrationManager;
        }

        private void OnEnable()
        {
            _smallButton.onClick.AddListener(DoSmall);
            _mediumButton.onClick.AddListener(DoMedium);
            _largeButton.onClick.AddListener(DoLarge);
        }

        private void OnDisable()
        {
            _smallButton.onClick.RemoveListener(DoSmall);
            _mediumButton.onClick.RemoveListener(DoMedium);
            _largeButton.onClick.RemoveListener(DoLarge);
        }

        private void DoSmall()
        {
            _vibrationManager.DoVibration(VibrationType.Light);
        }
        private void DoMedium()
        {
            _vibrationManager.DoVibration(VibrationType.Medium);
        }
        private void DoLarge()
        {
            _vibrationManager.DoVibration(VibrationType.Heavy);
        }
    }
}
