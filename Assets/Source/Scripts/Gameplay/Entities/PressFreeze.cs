using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class PressFreeze : PressHandler
    {
        public Action<bool> OnStatusChange;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _stopSpeed;
        private Rigidbody2D _playerRb;

        protected override void StopMove()
        {
            _collider.enabled = false;
            OnStatusChange?.Invoke(false);
        }

        protected override void UpdateMove(float delta)
        {
            _collider.enabled = true;
            OnStatusChange?.Invoke(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                _playerRb = player.GetComponent<Rigidbody2D>();
                player.SetGravity(false);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_playerRb != null)
                _playerRb.velocity = DecreseVelocity(_playerRb.velocity);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.SetGravity(true);
                _playerRb = null;
            }
        }

        private Vector3 DecreseVelocity(Vector3 velocity)
        {
            float speed = velocity.magnitude;

            if (speed <= 0f)
                return Vector3.zero;

            speed -= _stopSpeed * Time.deltaTime;
            speed = Mathf.Max(speed, 0f);

            return velocity.normalized * speed;
        }
    }
}
