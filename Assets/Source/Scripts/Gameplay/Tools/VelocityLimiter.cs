using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VelocityLimiter : MonoBehaviour
    {
        [SerializeField] private float _maxVelocity = 10f;
        [SerializeField] private bool _ignoreGravityAxis = true;

        private Rigidbody2D _body;
        private Vector2 _gravityDir;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _gravityDir = Physics2D.gravity.normalized;
        }

        private void FixedUpdate()
        {
            Vector2 velocity = _body.velocity;

            if (_ignoreGravityAxis)
            {
                Vector2 gravityComponent =
                    Vector2.Dot(velocity, _gravityDir) * _gravityDir;

                Vector2 lateralComponent =
                    velocity - gravityComponent;

                if (lateralComponent.magnitude > _maxVelocity)
                {
                    lateralComponent =
                        lateralComponent.normalized * _maxVelocity;
                }

                _body.velocity = gravityComponent + lateralComponent;
            }
            else
            {
                if (velocity.magnitude > _maxVelocity)
                {
                    _body.velocity = velocity.normalized * _maxVelocity;
                }
            }
        }
    }
}
