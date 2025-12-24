using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ConturExitInvoker : MonoBehaviour
    {
        private SignalBus _signalBus;
        private bool _isActive = true;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                if(_isActive)
                {
                    _signalBus.Fire(new SignalPlayerExitContour(player));
                    _isActive = false;
                }
            }
        }
    }
}
