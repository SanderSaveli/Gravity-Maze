using R3;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public interface IAudioManager
    {
        public ReactiveProperty<bool> IsMusicOn { get; }
        public ReactiveProperty<bool> IsSoundOn { get; }

        public void ChangeMainAudio(AudioClip audioClip);
        public void PlaySoundByType(SoundTypes type);
    }
}
