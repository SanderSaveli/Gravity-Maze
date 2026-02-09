using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class FpsSetter : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
    }
}
