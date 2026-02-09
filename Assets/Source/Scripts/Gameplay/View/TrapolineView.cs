using DG.Tweening;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class TrapolineView : MonoBehaviour
    {
        [SerializeField] private Trampoline _trampoline;
        [SerializeField] private float _maxScale;
        [SerializeField] private float _timeBeforeMaxScale;
        private float _normalScale;

        private void OnEnable()
        {
            _normalScale = transform.localScale.y;
            _trampoline.OnPump += AnimateReload;
        }

        private void OnDisable()
        {
            _trampoline.OnPump -= AnimateReload;
        }

        private void AnimateReload()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScaleY(_maxScale, _timeBeforeMaxScale))
                .Append(transform.DOScaleY(_normalScale, _trampoline.ReloadDuration - _timeBeforeMaxScale))
                .SetLink(gameObject);
        }
    }
}
