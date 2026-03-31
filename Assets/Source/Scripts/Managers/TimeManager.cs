using R3;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class TimeManager : MonoBehaviour, ITimeManager
    {
        public float CurrentTimeScale { get; private set; }
        private CompositeDisposable _compositeDisposable;
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            ChangeTimeMode(appSettings.TimeMode.Value);
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _appSettings.TimeMode.Subscribe(ChangeTimeMode).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        public void ChangeTimeMode(TimeMode timeMode)
        {
            if (_appSettings.TimeMode.Value != timeMode)
            {
                _appSettings.TimeMode.Value = timeMode;
            }

            switch (timeMode)
            {
                case TimeMode.normal:
                    CurrentTimeScale = 1f;
                    break;
                case TimeMode.fast:
                    CurrentTimeScale = 1.5f;
                    break;
                default:
                    CurrentTimeScale = 1;
                    break;
            }

            Time.timeScale = CurrentTimeScale;
        }

    }
}
