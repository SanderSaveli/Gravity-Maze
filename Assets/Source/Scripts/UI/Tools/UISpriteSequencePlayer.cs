using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace SanderSaveli.GravityMaze
{
    public class UISpriteSequencePlayer : MonoBehaviour
    {
        public event Action CycleCompleted;

        [Header("Components")]
        [SerializeField] private Image _image;

        [Header("Parameters")]
        [SerializeField] private Sprite[] _frames;
        [SerializeField] private float _framesPerSecond = 60f;
        [SerializeField] private float _cycleDelay = 0.5f;
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private bool _loop = true;
        [SerializeField] private bool _useUnscaledTime = true;

        private int _frameIndex;
        private float _frameTimer;
        private float _cycleDelayTimer;
        private bool _isPlaying;
        private bool _isWaitingForNextCycle;

        private void Awake()
        {
            if (_image == null)
                _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (_playOnEnable)
                PlayFromStart();
        }

        private void Update()
        {
            if (!_isPlaying || _frames == null || _frames.Length == 0 || _framesPerSecond <= 0f)
                return;

            float deltaTime = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            if (_isWaitingForNextCycle)
            {
                UpdateCycleDelay(deltaTime);
                return;
            }

            float frameDuration = 1f / _framesPerSecond;
            _frameTimer += deltaTime;

            if (_frameTimer < frameDuration)
                return;

            _frameTimer = 0f;
            AdvanceFrame();
        }

        public void PlayFromStart()
        {
            if (_image == null)
            {
                Debug.LogError("UI sprite sequence player has no Image component.");
                return;
            }

            if (_frames == null || _frames.Length == 0)
            {
                Debug.LogError("UI sprite sequence player has no frames.");
                return;
            }

            _frameIndex = 0;
            _frameTimer = 0f;
            _cycleDelayTimer = 0f;
            _isPlaying = true;
            _isWaitingForNextCycle = false;
            ApplyCurrentFrame();
        }

        public void Stop()
        {
            _isPlaying = false;
            _isWaitingForNextCycle = false;
        }

        private void UpdateCycleDelay(float deltaTime)
        {
            _cycleDelayTimer -= deltaTime;

            if (_cycleDelayTimer > 0f)
                return;

            _frameIndex = 0;
            _frameTimer = 0f;
            _isWaitingForNextCycle = false;
            ApplyCurrentFrame();
        }

        private void AdvanceFrame()
        {
            _frameIndex++;

            if (_frameIndex >= _frames.Length)
            {
                if (!_loop)
                {
                    _frameIndex = _frames.Length - 1;
                    _isPlaying = false;
                    CycleCompleted?.Invoke();
                    return;
                }
                _image.sprite = _frames.First();
                CycleCompleted?.Invoke();

                if (_cycleDelay > 0f)
                {
                    _frameIndex = _frames.Length - 1;
                    _cycleDelayTimer = _cycleDelay;
                    _isWaitingForNextCycle = true;
                    return;
                }

                _frameIndex = 0;
            }

            ApplyCurrentFrame();
        }

        private void ApplyCurrentFrame()
        {
            _image.sprite = _frames[_frameIndex];
        }
    }
}
