using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallHitSoundPlayer : MonoBehaviour
    {
        [SerializeField] private float _angleThreshold = 35f;
        [SerializeField] private float _speedThreshold = 1f;
        [SerializeField] private float _cooldown = 0.1f;
        [SerializeField] private bool _isMakeVibration = true;

        private Rigidbody2D _rb;
        private IAudioManager _audioManager;

        private Vector2 _lastVelocity;
        private float _lastSoundTime;
        private IVibrationManager _vibrationManager;

        [Inject]
        public void Construct(IAudioManager audioManager, IVibrationManager vibrationManager)
        {
            _audioManager = audioManager;
            _vibrationManager = vibrationManager;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector2 currentVelocity = _rb.velocity;

            if (currentVelocity.magnitude > _speedThreshold &&
                _lastVelocity.magnitude > _speedThreshold)
            {
                float angle = Vector2.Angle(_lastVelocity, currentVelocity);

                if (angle > _angleThreshold &&
                    Time.time - _lastSoundTime > _cooldown)
                {
                    _lastSoundTime = Time.time;
                    _audioManager.PlaySoundByType(SoundTypes.BallHit);
                    if(_isMakeVibration)
                    {
                        _vibrationManager.DoVibration(VibrationType.Light);
                    }
                }
            }

            _lastVelocity = currentVelocity;
        }
    }
}