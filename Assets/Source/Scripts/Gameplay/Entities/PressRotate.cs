using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class PressRotate : PressHandler
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;

        protected override void StopMove()
        {
            _rigidbody.angularVelocity = 0;
        }

        protected override void UpdateMove(float delta)
        {
            float angularVelocity = _speed * delta;
            _rigidbody.angularVelocity = angularVelocity;
        }
    }
}
