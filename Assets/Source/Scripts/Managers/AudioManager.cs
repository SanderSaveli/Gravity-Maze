using R3;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        public ReactiveProperty<bool> IsMusicOn { get; private set; }

        public ReactiveProperty<bool> IsSoundOn { get; private set; }

        private IAppSettings _appSettings;
        private CompositeDisposable _compositeDisposable;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
            _appSettings = appSettings;
            IsMusicOn = appSettings.IsMusicOn;
            IsSoundOn = appSettings.IsSoundOn;
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            IsMusicOn.Subscribe(HandleChangeMusic).AddTo(_compositeDisposable);
            IsSoundOn.Subscribe(HandleChangeSound).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        public void ChangeMainAudio(AudioClip audioClip)
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }

        public void PlaySoundByType(SoundTypes type)
        {

        }

        private void HandleChangeMusic(bool isOn)
        {
            _musicSource.mute = !isOn;
        }

        private void HandleChangeSound(bool isOn)
        {
            _soundSource.mute = !isOn;
        }
    }
}
