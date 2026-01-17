using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class StaticElement : MonoBehaviour
    {
        private IRotationManager _rotationManager;


        [Inject]
        public void Construct(IRotationManager rotationManager)
        {
            _rotationManager = rotationManager;
        }

        private void OnEnable()
        {
            _rotationManager.OnRotatonChange += ChngeRotatin;
        }

        private void OnDisable()
        {
            _rotationManager.OnRotatonChange -= ChngeRotatin;
        }

        private void ChngeRotatin(float targetRotation)
        {
            transform.rotation = Quaternion.Euler(0, 0, targetRotation);
        }
    }
}
