using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class WaveView : MonoBehaviour
    {
        public Action<WaveView> OnRemoved { get; set; }

        [Header("Components")]
        [SerializeField] private Image _image;

        [Header("params")]
        [SerializeField] private float _maxScale = 2f;
        [SerializeField] private float _animationDuration = 2f;

        private void OnDisable()
        {
            ReleaseWave();
        }

        private void Start()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScale(_maxScale, _animationDuration))
                .Join(_image.DOFade(0, _animationDuration))
                .SetLink(gameObject)
                .OnComplete(()=> ReleaseWave());
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }

        private void ReleaseWave()
        {
            OnRemoved?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
