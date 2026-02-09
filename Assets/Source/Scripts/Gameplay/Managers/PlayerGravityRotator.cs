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
        private bool _useParentGravity;
        private Transform _parent;

        [Inject]
        public void Construct(IRotationManager rotationManager, IGameplayConfig gameplayConfig)
        {
            _rotationManager = rotationManager;
            _gravityStrength = gameplayConfig.GravityForce;
        }


        public void AttachTo(Transform newParent)
        {
            transform.SetParent(newParent, true);
            _parent = newParent;
            _useParentGravity = true;

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
            Vector2 gravityDir;

            if (_useParentGravity && _parent != null)
            {
                gravityDir = _parent.TransformDirection(Vector2.down);
            }
            else
            {
                gravityDir = Quaternion.Euler(0f, 0f, _targetRotation) * Vector2.down;
            }
            gravityDir.Normalize();
            Vector2 gravityForce = gravityDir * _gravityStrength * _rb.mass;
            _rb.AddForce(gravityForce, ForceMode2D.Force);
        }
    }
}
