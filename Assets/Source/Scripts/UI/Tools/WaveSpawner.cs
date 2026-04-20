using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class WaveSpawner : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private WaveView _waveView;
        [SerializeField] private Transform _waveParent;
        [SerializeField] private Transform _emitor;

        [Header("Params")]
        [Min(0.1f)]
        [SerializeField] private float _spawnDelay;
        [SerializeField] protected float _initialDelay;
        [SerializeField] protected float _emitorScaleUpDuration;
        [SerializeField] protected Ease _emitorScaleUpEase;
        [SerializeField] protected float _emitorScaleDownDuration;
        [SerializeField] protected Ease _emitorScaleDownEase;
        [SerializeField] protected float _emitorMaxScale;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(SpawnRoutine());
        }

        private void OnDisable()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = null;
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSecondsRealtime(_initialDelay);
            while (true)
            {
                Spawn();
                yield return new WaitForSecondsRealtime(_spawnDelay);
            }
        }

        private void Spawn()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(_emitor.DOScale(_emitorMaxScale, _emitorScaleUpDuration))
                .SetEase(_emitorScaleUpEase)
                .Append(_emitor.DOScale(1, _emitorScaleDownDuration)
                .SetEase(_emitorScaleDownEase)
                .OnComplete(() => Instantiate(_waveView, _waveParent)))
                .SetLink(gameObject);
        }
    }
}
