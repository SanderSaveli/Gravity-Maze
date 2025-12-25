using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerGravityRotator : MonoBehaviour
    {
        private IRotationManager _rotationManager;

        private Rigidbody2D _rb;
        private float _targetRotation;

        private float _gravityStrength = 9.81f;

        [Inject]
        public void Construct(IRotationManager rotationManager, IGameplayConfig gameplayConfig)
        {
            _rotationManager = rotationManager;
            _gravityStrength = gameplayConfig.GravityForce;
        }


        public void AttachTo(Transform newParent)
        {
            _rb.simulated = false;
            Vector2 velocity = _rb.velocity;
            transform.SetParent(newParent, true);
            _rb.velocity = velocity;
            _rb.simulated = true;
            Debug.Log("Change Parent");
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rotationManager.OnRotatonChange += HandleRotationChange;
        }

        private void OnDisable()
        {
            _rotationManager.OnRotatonChange -= HandleRotationChange;
        }

        private void HandleRotationChange(float value)
        {
            _targetRotation = value;
        }

        private void FixedUpdate()
        {
            Vector2 gravityDir = Quaternion.Euler(0f, 0f, _targetRotation) * Vector2.down;
            Vector2 gravityForce = gravityDir * _gravityStrength * _rb.mass;
            _rb.AddForce(gravityForce, ForceMode2D.Force);
        }
    }
}
