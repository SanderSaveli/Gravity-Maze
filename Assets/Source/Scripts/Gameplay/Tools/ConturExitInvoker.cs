using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ConturExitInvoker : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                _signalBus.Fire(new SignalPlayerExitContour(player));
            }
        }
    }
}
