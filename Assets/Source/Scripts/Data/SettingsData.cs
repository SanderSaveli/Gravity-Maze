using System;

namespace SanderSaveli.GravityMaze
{
    [Serializable]
    public class SettingsData
    {
        public bool is_music_on;
        public bool is_sound_on;
        public bool is_vibration_on;
        public bool is_ads_removed;
        public Language language;
        public ColorSheme color;
        public TimeMode time_mode;
    }
}
