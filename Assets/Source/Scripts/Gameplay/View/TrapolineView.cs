using DG.Tweening;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class TrapolineView : MonoBehaviour
    {
        [SerializeField] private Trampoline _trampoline;
        [SerializeField] private float _maxScale;
        [SerializeField] private float _timeBeforeMaxScale;
        [SerializeField] private VibrationType _vibrationType;
        private IVibrationManager _vibrationManager;
        private IAudioManager _audioManager;
        private float _normalScale;

        [Inject]
        public void Construct(IVibrationManager vibrationManager, IAudioManager audioManager)
        {
            _vibrationManager = vibrationManager;
            _audioManager = audioManager;
        }

        private void OnEnable()
        {
            _normalScale = transform.localScale.y;
            _trampoline.OnPump += AnimateReload;
        }

        private void OnDisable()
        {
            _trampoline.OnPump -= AnimateReload;
        }

        private void AnimateReload()
        {
            _vibrationManager.DoVibration(_vibrationType);
            _audioManager.PlaySoundByType(SoundTypes.TrampolineActivate);
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScaleY(_maxScale, _timeBeforeMaxScale))
                .Append(transform.DOScaleY(_normalScale, _trampoline.ReloadDuration - _timeBeforeMaxScale))
                .SetLink(gameObject);
        }
    }
}
