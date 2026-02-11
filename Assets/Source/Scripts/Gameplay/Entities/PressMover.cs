using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class PressMover : PressHandler
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [Header("Point 1")]
        [SerializeField] private Vector2 _position1;
        [SerializeField] private float _rotation1;

        [Header("Point 2")]
        [SerializeField] private Vector2 _position2;
        [SerializeField] private float _rotation2;

        [Header("Movement")]
        [Tooltip("Time in seconds to move from Point 1 to Point 2")]
        [SerializeField] private float _cycleDuration = 2f;

        private Rigidbody2D _rb;

        private float _t;
        private int _direction = 1;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void UpdateMove(float delta)
        {
            UpdateProgress();
            Move();
            Rotate();
        }

        protected override void StopMove()
        {

        }

        private void UpdateProgress()
        {
            if (_cycleDuration <= 0f)
                return;

            float delta = Time.fixedDeltaTime / _cycleDuration;
            _t += delta * _direction;

            if (_t >= 1f)
            {
                _t = 1f;
                _direction = -1;
            }
            else if (_t <= 0f)
            {
                _t = 0f;
                _direction = 1;
            }
        }

        private void Move()
        {
            Vector2 target = Vector2.Lerp(_position1, _position2, _t);
            _rb.MovePosition(target);
        }

        private void Rotate()
        {
            float rot = Mathf.LerpAngle(_rotation1, _rotation2, _t);
            _rb.MoveRotation(rot);
        }

        public void RecordPoint1()
        {
            _position1 = transform.localPosition;
            _rotation1 = transform.localEulerAngles.z;
        }

        public void RecordPoint2()
        {
            _position2 = transform.localPosition;
            _rotation2 = transform.localEulerAngles.z;
        }
    }
}
