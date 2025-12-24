using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RigidbodyForceDebug : MonoBehaviour
    {
        [Header("Debug Settings")]
        [SerializeField] private float _forceScale = 0.05f;
        [SerializeField] private float _gravityScale = 0.05f;
        [SerializeField] private bool _drawInPlayMode = true;

        private Rigidbody2D _rb;
        private Vector2 _lastVelocity;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _lastVelocity = _rb.velocity;
        }

        private void FixedUpdate()
        {
            _lastVelocity = _rb.velocity;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !_drawInPlayMode)
                return;

            if (_rb == null)
                _rb = GetComponent<Rigidbody2D>();

            Vector3 position = _rb.worldCenterOfMass;

            // ===== √–¿¬»“¿÷»ﬂ =====
            if (_rb.gravityScale != 0f)
            {
                Vector2 gravityForce =
                    Physics2D.gravity * _rb.gravityScale * _rb.mass;

                DrawArrow(position, gravityForce * _gravityScale, Color.blue);
            }

            // ===== –≈«”À‹“»–”ﬁŸ¿ﬂ —»À¿ =====
            Vector2 acceleration =
                (_rb.velocity - _lastVelocity) / Time.fixedDeltaTime;

            Vector2 netForce =
                acceleration * _rb.mass;

            DrawArrow(position, netForce * _forceScale, Color.red);
        }

        // ===== –»—Œ¬¿Õ»≈ —“–≈À » =====
        private void DrawArrow(Vector3 start, Vector2 vector, Color color)
        {
            if (vector.sqrMagnitude < 0.0001f)
                return;

            Gizmos.color = color;

            Vector3 end = start + (Vector3)vector;
            Gizmos.DrawLine(start, end);

            Vector3 dir = vector.normalized;
            float headLength = vector.magnitude * 0.25f;
            float headAngle = 25f;

            Quaternion rot = Quaternion.Euler(0, 0, headAngle);
            Quaternion rotInv = Quaternion.Euler(0, 0, -headAngle);

            Vector3 right = rot * -dir;
            Vector3 left = rotInv * -dir;

            Gizmos.DrawLine(end, end + right * headLength);
            Gizmos.DrawLine(end, end + left * headLength);
        }
    }
}

