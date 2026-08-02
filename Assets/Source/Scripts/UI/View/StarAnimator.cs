using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class StarAnimator : MonoBehaviour
    {
        [SerializeField] private List<Transform> _stars;

        [SerializeField] private float _begin_delay;

        [SerializeField] private float _delay_1;
        [SerializeField] private float _delay_2;
        [SerializeField] private float _delay_3;
        [SerializeField] private float _delay_4;

        [Header("in")]
        [SerializeField] private float _inDuration;
        [SerializeField] private float _inDuration_1;
        [SerializeField] private float _beginScale_1;
        [SerializeField] private float _beginScale_2;
        [SerializeField] private Ease _inEase;
        [SerializeField] private float _maxScale_1;
        [SerializeField] private float _maxScale_2;
        [SerializeField] private float _maxScale_3;
        [SerializeField] private float _rotate;

        [Header("Out")]
        [SerializeField] private float _outDuration;
        [SerializeField] private Ease _outEase;

        private void OnEnable()
        {
            _ = AnimateStars();
        }

        public async UniTask AnimateStars()
        {
            foreach (Transform t in _stars)
            {
                t.localScale = Vector3.zero;
            }
            await UniTask.WaitForSeconds(_begin_delay, true);
            AnimateStar(_stars[0], _maxScale_1, _inDuration, _beginScale_1);
            await UniTask.WaitForSeconds(_delay_1, true);
            AnimateStar(_stars[1], _maxScale_2, _inDuration_1, _beginScale_2);
            await UniTask.WaitForSeconds(_delay_2, true);
            AnimateStar(_stars[2], _maxScale_2, _inDuration_1, _beginScale_2);
            await UniTask.WaitForSeconds(_delay_2, true);
            AnimateStar(_stars[3], _maxScale_3, _inDuration, _beginScale_2);
            await UniTask.WaitForSeconds(_delay_2, true);
            AnimateStar(_stars[4], _maxScale_3, _inDuration, _beginScale_2);
        }

        private void AnimateStar(Transform transform, float maxScaleLevel, float inDuration, float startScale)
        {
            Vector3 rotate = new Vector3(0, 0, _rotate);
            transform.rotation = Quaternion.Euler(rotate);
            transform.localScale = new Vector3(startScale, startScale, startScale);

            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScale(maxScaleLevel, inDuration).SetEase(_inEase))
                .Join(transform.DORotate(Vector3.zero, inDuration))
                .Append(transform.DOScale(1, _outDuration).SetEase(_outEase)
                .SetLink(transform.gameObject));
        }
    }
}
