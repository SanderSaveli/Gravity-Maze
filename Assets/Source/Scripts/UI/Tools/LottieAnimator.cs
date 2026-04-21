using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(LottiePlayer))]
    public class LottieAnimator : MonoBehaviour, ISelectable
    {
        public Action OnStopPlay;

        [SerializeField] private LottiePlayer _lottiePlayer;
        [Header("Animations")]
        [SerializeField] private TextAsset _onAnimation;
        [SerializeField] private TextAsset _offAnimation;

        public bool IsSelected { get; private set; }
        private bool _isPlaying = false;    

        private async void Update()
        {
            if (_isPlaying)
            {
                if (!_lottiePlayer.IsPlaying)
                {
                    await UniTask.Yield();
                    if (!_lottiePlayer.IsPlaying && _isPlaying)
                    {
                        _isPlaying = false;
                        //OnStopPlay?.Invoke();
                    }
                }
            }
        }

        private void Awake()
        {
            if (_lottiePlayer == null)
                _lottiePlayer = GetComponent<LottiePlayer>();
        }

        public void Select()
        {
            if (IsSelected) return;
            _isPlaying = true;
            IsSelected = true;
            PlayOn();
        }

        public void Deselect()
        {
            if (!IsSelected) return;
            _isPlaying = true;
            IsSelected = false;
            PlayOff();
        }

        private void PlayOn()
        {
            if (_onAnimation == null) return;

            _lottiePlayer.Stop();

            _lottiePlayer.LoadAnimationFromTextAsset(_onAnimation);

            _lottiePlayer.Play();
        }

        private void PlayOff()
        {
            Debug.Log("Off");
            if (_offAnimation == null) return;

            _lottiePlayer.Stop();
            _lottiePlayer.LoadAnimationFromTextAsset(_offAnimation);
            _lottiePlayer.Play();
        }
    }
}
