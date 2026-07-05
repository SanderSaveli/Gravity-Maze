using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class HomeScreenAnimationDirector : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private UISpriteSequencePlayer _sequencePlayer;
        [SerializeField] private UIFlyToTargetAnimator _flyAnimator;
        [SerializeField] private RectTransform _startPoint;
        [SerializeField] private RectTransform _targetPoint;
        [SerializeField] private TriggeredWaveSpawner _waveSpawner;

        private void OnEnable()
        {
            _sequencePlayer.CycleCompleted += HandleSequenceCompleted;
        }

        private void OnDisable()
        {
            _sequencePlayer.CycleCompleted -= HandleSequenceCompleted;
        }

        private void HandleSequenceCompleted()
        {
            _flyAnimator.Play(_startPoint, _targetPoint, HandleFlyCompleted);
        }

        private void HandleFlyCompleted()
        {
            _waveSpawner.SpawnWave();
        }
    }
}
