using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public abstract class SoundPlayer : MonoBehaviour
    {
        public abstract AudioClip PlayHitSound();
    }
}
