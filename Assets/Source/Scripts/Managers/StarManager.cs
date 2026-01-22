using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class StarManager : MonoBehaviour, IStarManager
    {
        public bool IsStarCollect => _collectedStars.Count > 0;
        public IReadOnlyList<GameStar> CollectedStars => _collectedStars;

        private List<GameStar> _collectedStars;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _collectedStars = new List<GameStar>();
            _signalBus.Subscribe<SignalStarCollected>(HandleStarCollect);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<SignalStarCollected>(HandleStarCollect);
        }

        private void HandleStarCollect(SignalStarCollected ctx)
        {
            _collectedStars.Add(ctx._gameStar);
        }
    }
}
