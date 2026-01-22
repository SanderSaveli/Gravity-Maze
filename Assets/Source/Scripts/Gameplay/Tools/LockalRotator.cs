using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LockalRotator : MonoBehaviour
    {
        private IRotationManager _rotationManager;

        [Inject]
        public void Construct(IRotationManager rotationManager)
        {

        }


    }
}
