using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class WinVibro : MonoBehaviour
    {
        [SerializeField] VibrationType _type;

        private IVibrationManager _vibrationManager;
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus, IVibrationManager vibrationManager)
        {
            _signalBus = signalBus;
            _vibrationManager = vibrationManager;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalGameEnd>(DoVibration);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalGameEnd>(DoVibration);
        }

        private void DoVibration()
        {
            _vibrationManager.DoVibration(_type);
        }
    }
}
