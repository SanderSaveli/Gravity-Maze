using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(PortalNormal))]
    public class PortalTeleportator : MonoBehaviour
    {
        public PortalTeleportator LinkedPortal { get; set; }
        public Collider2D BackWall => _backWall;
        public bool IsActive { get; private set; }

        [Header("Components")]
        [SerializeField] private PortalController _controller;
        [SerializeField] private Collider2D _backWall;

        [Header("Settings")]
        [SerializeField] private float _minVelocity = 10f;
        [SerializeField] private float _maxVelocity = 200f;
        [SerializeField] private float _minTimeBetweenTP = 0.1f;
        
        private float _remainingTimeBetweenTP = 0f;
        private bool _isCanTeleport => _remainingTimeBetweenTP <= 0;

        private void Reset()
        {
            _controller = GetComponent<PortalController>();
        }

        private void Update()
        {
            if (_remainingTimeBetweenTP > 0)
            {
                _remainingTimeBetweenTP = Mathf.Clamp(_remainingTimeBetweenTP - Time.deltaTime, 0, _minTimeBetweenTP);
            }
        }

        private void OnEnable()
        {
            _controller.OnPrepareForTeleport += SetActive;
        }

        private void OnDisable()
        {
            _controller.OnPrepareForTeleport -= SetActive;
        }

        private void SetActive(bool isActive)
        {
            _backWall.enabled = !isActive;
        }

        public void UpdateTPTime()
        {
            _remainingTimeBetweenTP = _minTimeBetweenTP;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!_isCanTeleport) return;
            if (_controller == null || LinkedPortal == null) return;

            var rb = collision.attachedRigidbody;
            if (rb == null) return;

            Vector2 objCenter = rb.position;

            var portal = _controller.PortalNormal;

            Vector2 normal = portal.PortalNormalVector.normalized;
            Vector2 tangent = (Vector2)portal.transform.up.normalized;

            Vector2 planePos = portal.PlanePosition;

            // положение центра относительно портала
            Vector2 relative = objCenter - planePos;

            float normalDist = Vector2.Dot(relative, normal);
            float tangentDist = Vector2.Dot(relative, tangent);

            // получаем границы линии портала
            float halfWidth = portal.width * 0.5f;

            bool insideLine = Mathf.Abs(tangentDist) <= halfWidth;

            // если центр пересёк линию портала
            if (insideLine && normalDist <= 0f)
            {
                Teleport(rb, objCenter);
            }
        }

        private void Teleport(Rigidbody2D rb, Vector2 objCenter)
        {
            LinkedPortal._backWall.enabled = false;

            float portalAngle = Mathf.Atan2(_controller.PortalNormal.PortalNormalVector.y, _controller.PortalNormal.PortalNormalVector.x) * Mathf.Rad2Deg;
            float linkedAngle = Mathf.Atan2(LinkedPortal._controller.PortalNormal.PortalNormalVector.y, LinkedPortal._controller.PortalNormal.PortalNormalVector.x) * Mathf.Rad2Deg;

            float deltaAngle = linkedAngle - portalAngle + 180f;

            UpdateTPTime();
            LinkedPortal.UpdateTPTime();

            // позиция игрока после телепортации — центр плоскости linkedPortal
            rb.position = LinkedPortal._controller.PortalNormal.PlanePosition;

            // скорость с поворотом
            Vector2 rotatedVel = Rotate(rb.velocity, deltaAngle);
            float speed = Mathf.Clamp(rotatedVel.magnitude, _minVelocity, _maxVelocity);
            rotatedVel = rotatedVel.normalized * speed;

            rb.velocity = rotatedVel;
        }

        private Vector2 Rotate(Vector2 v, float angleDegrees)
        {
            float rad = angleDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            return new Vector2(
                v.x * cos - v.y * sin,
                v.x * sin + v.y * cos
            );
        }
    }
}