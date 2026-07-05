using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class TriggeredWaveSpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private WaveView _waveView;
        [SerializeField] private Transform _waveParent;
        private List<WaveView> _waveViews = new List<WaveView>();
        private Color _waveColor;

        public void SpawnWave()
        {
            if (_waveView == null)
            {
                Debug.LogError("Triggered wave spawner has no wave view prefab.");
                return;
            }

            Transform parent = _waveParent != null ? _waveParent : transform;
            WaveView wave = Instantiate(_waveView, parent);
            _waveViews.Add(wave);
            wave.OnRemoved += RemoveWave;
            wave.SetColor(_waveColor);
        }

        private void RemoveWave(WaveView wave)
        {
            _waveViews.Remove(wave);
        }

        public void SetWaveColor(Color color)
        {
            _waveColor = color;
            foreach (var item in _waveViews)
            {
                item.SetColor(_waveColor);
            }
        }
    }
}
