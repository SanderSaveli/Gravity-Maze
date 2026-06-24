using R3;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        public ReactiveProperty<bool> IsMusicOn { get; private set; }

        public ReactiveProperty<bool> IsSoundOn { get; private set; }

        private CompositeDisposable _compositeDisposable;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;
        [Space]
        [SerializeField] private SoundPlayer _soundPlayer;
        [Space]
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _starCollected;
        [SerializeField] private AudioClip _trampoline;
        [SerializeField] private AudioClip _winGame;

        [Inject]
        public void Construct(IAppSettings appSettings)
        {
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
            switch (type)
            {
                case SoundTypes.BallHit:
                    PlaySound(_soundPlayer.PlayHitSound());
                    break;
                case SoundTypes.TrampolineActivate:
                    PlaySound(_trampoline);
                    break;
                case SoundTypes.StarCollected:
                    PlaySound(_starCollected);
                    break;
                case SoundTypes.WinGame:
                    PlaySound(_winGame);
                    break;
                default:
                    break;
            }
        }

        private void PlaySound(AudioClip audioClip)
        {
            _soundSource.PlayOneShot(audioClip);
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
