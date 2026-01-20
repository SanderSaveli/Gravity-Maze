using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(LottiePlayer))]
    public class LottieAnimator : MonoBehaviour, ISelectable
    {
        [SerializeField] private LottiePlayer _lottiePlayer;
        [Header("Animations")]
        [SerializeField] private TextAsset _onAnimation;
        [SerializeField] private TextAsset _offAnimation;

        public bool IsSelected { get; private set; }

        private void Awake()
        {
            if (_lottiePlayer == null)
                _lottiePlayer = GetComponent<LottiePlayer>();
        }

        public void Select()
        {
            if (IsSelected) return;
            IsSelected = true;
            PlayOn();
        }

        public void Deselect()
        {
            if (!IsSelected) return;
            IsSelected = false;
            PlayOff();
        }

        private void PlayOn()
        {
            Debug.Log("On");
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
