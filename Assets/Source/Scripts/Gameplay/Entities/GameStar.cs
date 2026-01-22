using System;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    [RequireComponent(typeof(Collider2D))]
    public class GameStar : MonoBehaviour
    {
        public Action OnCollected { get; set; }
        public bool IsCollected { get; private set; }

        [SerializeField] private Collider2D _collider;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }


        private void Reset()
        {
            _collider = gameObject.GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player>(out _))
            {
                IsCollected = true;
                OnCollected?.Invoke();
                _signalBus.Fire(new SignalStarCollected(this));
            }
        }
    }
}
