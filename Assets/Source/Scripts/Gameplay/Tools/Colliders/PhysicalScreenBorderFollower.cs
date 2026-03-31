using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicalScreenBorderFollower : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _zOffset = 0f;

        private Rigidbody2D _rb;

        private Vector2 _targetPosition;
        private float _targetRotation;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            if (_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
            _targetPosition = _camera.transform.position;
            _targetRotation = _camera.transform.eulerAngles.z;
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_targetPosition);
            _rb.MoveRotation(_targetRotation);
        }
    }
}