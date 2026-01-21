using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class SettingsScreen : UiScreen
    {
        [Space]
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _languageButton;
        [SerializeField] private Button _scaleUIButton;
        [SerializeField] private Button _aboutUsButton;
        [SerializeField] private Button _removeAdsButton;

        protected override void SubscribeToEvents()
        {
            _soundButton.onClick.AddListener(HandleChangeSouds);
            _musicButton.onClick.AddListener(HandleChangeMusic);
            _languageButton.onClick.AddListener(HandleChangeLanguage);
            _scaleUIButton.onClick.AddListener(HandleChangeScale);
            _aboutUsButton.onClick.AddListener(HandleAboutUs);
            _removeAdsButton.onClick.AddListener(HandleRemoveAds);
            base.SubscribeToEvents();
        }

        protected override void UnsubscribeFromEvents()
        {
            _soundButton.onClick.RemoveListener(HandleChangeSouds);
            _musicButton.onClick.RemoveListener(HandleChangeMusic);
            _languageButton.onClick.RemoveListener(HandleChangeLanguage);
            _scaleUIButton.onClick.RemoveListener(HandleChangeScale);
            _aboutUsButton.onClick.RemoveListener(HandleAboutUs);
            _removeAdsButton.onClick.RemoveListener(HandleRemoveAds);
            base.UnsubscribeFromEvents();
        }

        private void HandleChangeSouds()
        {

        }

        private void HandleChangeMusic()
        {

        }

        private void HandleChangeLanguage()
        {

        }

        private void HandleChangeScale()
        {

        }

        private void HandleAboutUs()
        {

        }

        private void HandleRemoveAds()
        {

        }
    }
}
