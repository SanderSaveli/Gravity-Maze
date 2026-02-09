using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class Trampoline : MonoBehaviour
    {
        public Action OnPump { get; set; }
        public Action OnReloaded { get; set; }
        public Action<float> OnReloading { get; set; }
        public bool IsReadyy { get; set; }
        public float ReloadDuration => _reloadDuratin;
        [SerializeField] private float _reloadDuratin;
        [SerializeField] private float _force;
        private bool _isReady = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isReady)
            {
                return;
            }
            if (collision.gameObject.TryGetComponent<Player>(out _))
            {
                if (collision.gameObject.TryGetComponent(out Rigidbody2D rb))
                {
                    Pump(rb);
                }
            }
        }

        private void Pump(Rigidbody2D rb)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(CalculateForce());
            _isReady = false;
            OnPump?.Invoke();
            Reload();
        }

        private Vector3 CalculateForce()
        {
            return transform.up * _force;
        }

        private async void Reload()
        {
            float beforeReload = _reloadDuratin;
            while (beforeReload > 0)
            {
                OnReloading?.Invoke(1 - beforeReload / _reloadDuratin);
                beforeReload -= Time.deltaTime;
                await UniTask.Yield();
            }
            _isReady = true;
            OnReloaded?.Invoke();
        }
    }
}
