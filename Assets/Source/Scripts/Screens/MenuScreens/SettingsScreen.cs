using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class SettingsScreen : UiScreen
    {
        [Space]
        [SerializeField] private SelectButton _soundButton;
        [SerializeField] private SelectButton _musicButton;
        [SerializeField] private Button _languageButton;
        [SerializeField] private SelectButton _vibrationButton;
        [SerializeField] private Button _aboutUsButton;
        [SerializeField] private Button _removeAdsButton;
        private IAppSettings _appSettings;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void SubscribeToEvents()
        {
            _soundButton.SetState(_appSettings.IsSoundOn.Value);
            _musicButton.SetState(_appSettings.IsMusicOn.Value);
            _vibrationButton.SetState(_appSettings.IsVibrationOn.Value);

            _soundButton.OnSwitched += HandleChangeSouds;
            _musicButton.OnSwitched += HandleChangeMusic;
            _vibrationButton.OnSwitched += HandleChangeVibration;

            _languageButton.onClick.AddListener(HandleChangeLanguage);
            _aboutUsButton.onClick.AddListener(HandleAboutUs);
            _removeAdsButton.onClick.AddListener(HandleRemoveAds);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _soundButton.OnSwitched -= HandleChangeSouds;
            _musicButton.OnSwitched -= HandleChangeMusic;
            _vibrationButton.OnSwitched -= HandleChangeVibration;

            _languageButton.onClick.RemoveListener(HandleChangeLanguage);;
            _aboutUsButton.onClick.RemoveListener(HandleAboutUs);
            _removeAdsButton.onClick.RemoveListener(HandleRemoveAds);
            base.UnsubscribeFromEvents();
        }

        private void HandleChangeSouds(bool isOn)
        {
            _appSettings.IsSoundOn.Value = isOn;
        }

        private void HandleChangeMusic(bool isOn)
        {
            _appSettings.IsMusicOn.Value = isOn;
        }

        private void HandleChangeLanguage()
        {

        }

        private void HandleChangeVibration(bool isOn)
        {
            _appSettings.IsVibrationOn.Value = isOn;
        }

        private void HandleAboutUs()
        {

        }

        private void HandleRemoveAds()
        {

        }
    }
}
