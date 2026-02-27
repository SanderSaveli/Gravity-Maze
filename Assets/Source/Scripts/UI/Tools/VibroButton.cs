using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Button))]
    public class VibroButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private VibrationType _vibrationType;

        private IVibrationManager _vibrationManager;

        [Inject]
        public void Construct(IVibrationManager vibrationManager)
        {
            _vibrationManager = vibrationManager;
        }

        private void Reset()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(DoVibration);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(DoVibration);
        }

        private void DoVibration()
        {
            _vibrationManager.DoVibration(_vibrationType);
        }
    }
}
