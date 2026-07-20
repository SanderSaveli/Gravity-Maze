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
        public ReactiveProperty<bool> IsAdsRemoved { get; private set; }
        public ReactiveProperty<Language> Language { get; private set; }
        public ReactiveProperty<ColorSheme> ColorSheme { get; private set; }
        public ReactiveProperty<TimeMode> TimeMode { get; private set; }

        private const string SETTINGS_SAVE_PATH = "Save/AppSettings";
        private IStorageService _storageService;
        private CompositeDisposable _disposables;
        private bool _isApplyingLoadedData;

        [Inject]
        public void Construct()
        {
            IsMusicOn = new ReactiveProperty<bool>();
            IsSoundOn = new ReactiveProperty<bool>();
            IsVibrationOn = new ReactiveProperty<bool>();
            IsAdsRemoved = new ReactiveProperty<bool>();
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
            IsMusicOn.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
            IsSoundOn.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
            IsVibrationOn.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
            IsAdsRemoved.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
            Language.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
            ColorSheme.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
            TimeMode.Skip(1).Subscribe(_ => HandleSettingsChanged()).AddTo(_disposables);
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
            _isApplyingLoadedData = true;

            IsMusicOn.Value = settingsData.is_music_on;
            IsSoundOn.Value = settingsData.is_sound_on;
            IsVibrationOn.Value = settingsData.is_vibration_on;
            Language.Value = settingsData.language;
            IsAdsRemoved.Value = settingsData.is_ads_removed;
            ColorSheme.Value = settingsData.color;
            TimeMode.Value = settingsData.time_mode;

            _isApplyingLoadedData = false;
            Debug.Log("Language Setted: " + Language.Value);
        }

        private SettingsData CreateDefaultSettins()
        {
            SettingsData sd = new SettingsData();
            sd.is_music_on = true;
            sd.is_sound_on = true;
            sd.is_vibration_on = true;
            sd.is_ads_removed = false;
            sd.language = GetDefaultLanguage();
            sd.color = GravityMaze.ColorSheme.dark_9;
            sd.time_mode = GravityMaze.TimeMode.normal;
            return sd;
        }

        private Language GetDefaultLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    return GravityMaze.Language.ru;
                case SystemLanguage.German:
                    return GravityMaze.Language.de;
                case SystemLanguage.Spanish:
                    return GravityMaze.Language.es;
                case SystemLanguage.French:
                    return GravityMaze.Language.fr;
                case SystemLanguage.Italian:
                    return GravityMaze.Language.it;
                case SystemLanguage.Japanese:
                    return GravityMaze.Language.ja;
                case SystemLanguage.Korean:
                    return GravityMaze.Language.ko;
                case SystemLanguage.Portuguese:
                    return GravityMaze.Language.pt;
                default:
                    return GravityMaze.Language.en;
            }
        }

        private SettingsData GetCurrentData()
        {
            SettingsData sd = new SettingsData();
            sd.is_music_on = IsMusicOn.Value;
            sd.is_sound_on = IsSoundOn.Value;
            sd.is_vibration_on = IsVibrationOn.Value;
            sd.is_ads_removed |= IsAdsRemoved.Value;
            sd.language = Language.Value;
            Debug.Log("Language: " + Language.Value);
            sd.color = ColorSheme.Value;
            sd.time_mode = TimeMode.Value;
            return sd;
        }

        private void SaveCurrentData()
        {
            Debug.Log("Save Settings");
            _storageService.Save(SETTINGS_SAVE_PATH, GetCurrentData());
        }

        private void HandleSettingsChanged()
        {
            if (_isApplyingLoadedData)
                return;

            SaveCurrentData();
        }
    }
}
