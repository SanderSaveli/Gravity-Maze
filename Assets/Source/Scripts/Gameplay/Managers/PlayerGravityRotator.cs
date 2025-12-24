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
        private bool _isAttachedToCamera = false;
        private Transform _rotationCenter;
        private CameraLevelRotator _cameraRotator;

        [SerializeField] private float _gravityStrength = 9.81f;

        [Inject]
        public void Construct(IRotationManager rotationManager)
        {
            _rotationManager = rotationManager;
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

            if (_isAttachedToCamera)
            {
                Vector2 offset = _rb.position - (Vector2)_rotationCenter.position;
                float deltaAngle = _cameraRotator.AngularVelocityRad * Time.fixedDeltaTime;
                Vector2 rotatedOffset = RotateVector(offset, deltaAngle);

                _rb.position = (Vector2)_rotationCenter.position + rotatedOffset;

                _rb.velocity = RotateVector(_rb.velocity, deltaAngle);
            }
        }

        public void AttachToCamera(Transform rotationCenter, CameraLevelRotator cameraRotator)
        {
            _rotationCenter = rotationCenter;
            _cameraRotator = cameraRotator;
        }

        public void ExitContour()
        {
            _isAttachedToCamera = true;
            return;
        }

        private Vector2 RotateVector(Vector2 v, float angleRad)
        {
            float sin = Mathf.Sin(angleRad);
            float cos = Mathf.Cos(angleRad);
            return new Vector2(
                cos * v.x - sin * v.y,
                sin * v.x + cos * v.y
            );
        }
    }
}
