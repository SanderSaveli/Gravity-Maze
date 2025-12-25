using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class FpsSetter : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 80;
        }
    }
}
