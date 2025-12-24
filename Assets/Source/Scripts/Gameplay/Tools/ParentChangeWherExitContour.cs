using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ParentChangeWherExitContour : MonoBehaviour
    {
        [SerializeField] private Transform _targetParent;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<SignalPlayerExitContour>(HandleExitContour);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalPlayerExitContour>(HandleExitContour);
        }

        private void HandleExitContour(SignalPlayerExitContour ctx)
        {
            ctx.Player.transform.SetParent(_targetParent);

            PlayerGravityRotator rotator = ctx.Player.GetComponent<PlayerGravityRotator>();
            if (rotator != null)
            {
                rotator.ExitContour();
            }
            Debug.Log(ctx.Player.transform.parent.name);
        }
    }
}
