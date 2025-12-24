using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameEndPlatform : MonoBehaviour
    {
        private SignalBus _signalBus;
        private bool _isAlreadyWin;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.TryGetComponent(out Player player))
            {
                if(!_isAlreadyWin)
                {
                    _signalBus.Fire(new SignalGameEnd(true));
                    _isAlreadyWin = true;
                }
            }
        }
    }
}
