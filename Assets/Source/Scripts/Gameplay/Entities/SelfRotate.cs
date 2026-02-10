using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class SelfRotate : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;

        private void Update()
        {
            ApplyRotation();
        }

        private void ApplyRotation()
        {
            _rigidbody.angularVelocity = _speed;
        }
    }
}
