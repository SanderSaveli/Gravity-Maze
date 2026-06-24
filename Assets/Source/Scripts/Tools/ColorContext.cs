using System;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [Serializable]
    public class ColorContext
    {

        public int StarToUnlock => _starToUnlock;
        public int AdsToUnlock => _adsToUnlock;
        public ColorSheme ColorSheme => _colorSheme;
        public ColotUnlockType Type => _type;

        [Header("General")]
        [SerializeField] private ColorSheme _colorSheme;
        [SerializeField] private ColotUnlockType _type;

        [Header("Stars")]
        [SerializeField] private int _starToUnlock;

        [Header("Ads")]
        [SerializeField] private int _adsToUnlock;

        public ColorContext(ColorSheme colorSheme, ColotUnlockType type, int starToUnlock, int adsToUnlock)
        {
            _colorSheme = colorSheme;
            _type = type;
            _starToUnlock = starToUnlock;
            _adsToUnlock = adsToUnlock;
        }

        public ColorContext() { }
    }
}
