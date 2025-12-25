using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RotateBallWithContour2D : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _rotationCenter; 

        private Rigidbody2D _rb;
        private float _lastRotationZ;
        private bool _initialized;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        public void ApplyRotation(float currentRotationZ)
        {
            if (!_initialized)
            {
                _lastRotationZ = currentRotationZ;
                _initialized = true;
                return;
            }

            float deltaAngle = currentRotationZ - _lastRotationZ;
            _lastRotationZ = currentRotationZ;

            if (Mathf.Approximately(deltaAngle, 0f))
                return;

            RotateRigidbody(deltaAngle);
        }

        private void RotateRigidbody(float deltaAngle)
        {
            Vector2 center = _rotationCenter.position;

            // ===== œŒ¬Œ–Œ“ œŒ«»÷»» =====
            Vector2 offset = _rb.position - center;
            Vector2 rotatedOffset = RotateVector(offset, deltaAngle);
            _rb.position = center + rotatedOffset;

            // ===== œŒ¬Œ–Œ“ — Œ–Œ—“» =====
            _rb.velocity = RotateVector(_rb.velocity, deltaAngle);
        }

        private Vector2 RotateVector(Vector2 v, float angleDeg)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);

            return new Vector2(
                cos * v.x - sin * v.y,
                sin * v.x + cos * v.y
            );
        }
    }
}
