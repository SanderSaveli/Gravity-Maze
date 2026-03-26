using R3;
using SanderSaveli.UDK;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AppSettings : MonoBehaviour, IAppSettings
    {
        public ReactiveProperty<bool> IsMusicOn { get; private set; }
        public ReactiveProperty<bool> IsSoundOn { get; private set; }
        public ReactiveProperty<bool> IsVibrationOn { get; private set; }
        public ReactiveProperty<Language> Language { get; private set; }
        public ReactiveProperty<ColorSheme> ColorSheme { get; private set; }
        public ReactiveProperty<TimeMode> TimeMode { get; private set; }

        private const string SETTINGS_SAVE_PATH = "Save/AppSettings";
        private IStorageService _storageService;
        private CompositeDisposable _disposables;

        [Inject]
        public void Construct()
        {
            IsMusicOn = new ReactiveProperty<bool>();
            IsSoundOn = new ReactiveProperty<bool>();
            IsVibrationOn = new ReactiveProperty<bool>();
            Language = new ReactiveProperty<Language>();
            ColorSheme = new ReactiveProperty<ColorSheme>();
            TimeMode = new ReactiveProperty<TimeMode>();
        }

        private void Awake()
        {
            _storageService = new EncryptedJsonToFileStorageService();
            _storageService.Load<SettingsData>(SETTINGS_SAVE_PATH, OnDataLoaded);
        }

        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            IsMusicOn.Skip(1).Subscribe(_ => SaveCurrentData()).AddTo(_disposables);
            IsSoundOn.Skip(1).Subscribe(_ => SaveCurrentData()).AddTo(_disposables);
            IsVibrationOn.Skip(1).Subscribe(_ => SaveCurrentData()).AddTo(_disposables);
            Language.Skip(1).Subscribe(_ => SaveCurrentData()).AddTo(_disposables);
            ColorSheme.Skip(1).Subscribe(_ => SaveCurrentData()).AddTo(_disposables);
            TimeMode.Skip(1).Subscribe(_ => SaveCurrentData()).AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables?.Dispose();
            _disposables = null;
        }

        private void OnDataLoaded(SettingsData settingsData)
        {
            if (settingsData == null)
            {
                Debug.Log("Create new settings config");
                settingsData = CreateDefaultSettins();
                _storageService.Save(SETTINGS_SAVE_PATH, settingsData);
            }

            IsMusicOn.Value = settingsData.is_music_on;
            IsSoundOn.Value = settingsData.is_sound_on;
            IsVibrationOn.Value = settingsData.is_vibration_on;
            Language.Value = settingsData.language;
            ColorSheme.Value = settingsData.color;
            TimeMode.Value = settingsData.time_mode;
        }

        private SettingsData CreateDefaultSettins()
        {
            SettingsData sd = new SettingsData();
            sd.is_music_on = true;
            sd.is_sound_on = true;
            sd.is_vibration_on = true;
            sd.language = GravityMaze.Language.en;
            sd.color = GravityMaze.ColorSheme.dark_9;
            sd.time_mode = GravityMaze.TimeMode.normal;
            return sd;
        }

        private SettingsData GetCurrentData()
        {
            SettingsData sd = new SettingsData();
            sd.is_music_on = IsMusicOn.Value;
            sd.is_sound_on = IsSoundOn.Value;
            sd.is_vibration_on = IsVibrationOn.Value;
            sd.language = Language.Value;
            sd.color = ColorSheme.Value;
            sd.time_mode = TimeMode.Value;
            return sd;
        }

        private void SaveCurrentData()
        {
            Debug.Log("Save Settings");
            _storageService.Save(SETTINGS_SAVE_PATH, GetCurrentData());
        }
    }
}
