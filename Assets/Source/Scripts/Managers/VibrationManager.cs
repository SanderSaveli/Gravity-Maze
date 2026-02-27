using CandyCoded.HapticFeedback;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class VibrationManager : MonoBehaviour, IVibrationManager
    {
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void DoVibration(VibrationType type)
        {
            if(!_appSettings.IsVibrationOn.Value)
            {
                return;
            }

            switch (type)
            {
                case VibrationType.Light:
                    DoLightVibration();
                    break;
                case VibrationType.Medium:
                    DoMediumVibration();
                    break;
                case VibrationType.Heavy:
                    DoHeavyVibration();
                    break;
                default:
                    throw new System.NotImplementedException($"There is no case for {nameof(type)} = {type}");
            }
        }

        public void DoLightVibration()
        {
            HapticFeedback.LightFeedback();
        }

        public void DoMediumVibration()
        {
            HapticFeedback.MediumFeedback();
        }

        public void DoHeavyVibration()
        {
            HapticFeedback.HeavyFeedback();
        }
    }
}
