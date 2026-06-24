using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class IncreasingSoundPlayer : SoundPlayer
    {
        [SerializeField] private List<AudioClip> _hitSounds;
        [SerializeField] private float _decreaseSpeed;
        [SerializeField] private float _delayBeforeDecrease;

        private float _currentVolume;
        private float _hitSoundCount => _hitSounds.Count;
        private float _currentDelay;

        private void Update()
        {
            if(_currentDelay > 0)
            {
                _currentDelay -= Time.deltaTime;
            }
            else
            {
                _currentVolume -= _decreaseSpeed * Time.deltaTime;
                _currentVolume = Mathf.Clamp(_currentVolume, 0, 1);
            }
        }

        public override AudioClip PlayHitSound()
        {
            AudioClip currentClip = GetCurrentClip();
            _currentVolume += 1 / _hitSoundCount;
            _currentDelay = _delayBeforeDecrease;
            return currentClip;
        }

        private AudioClip GetCurrentClip()
        {
            float volume = Mathf.Lerp(0f, _hitSoundCount -1, _currentVolume);
            int index = Mathf.RoundToInt(volume);
            Debug.Log("Play Sound With Index " + index);
            return _hitSounds[index];
        }
    }
}
