using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class LevelTransitionScreenAnimator : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CounterAnimator _counterAnimator;
        [SerializeField] private GameObject _transitionScreen;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("params")]
        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;
        [SerializeField] private float _hideDelay;

        private void Start()
        {
            _transitionScreen.SetActive(false);
        }

        public async UniTask Show(int fromLevel, int toLevel)
        {
            await Show(fromLevel);
            await _counterAnimator.Animate(fromLevel, toLevel);
        }

        public async UniTask Show(int level)
        {
            _transitionScreen.SetActive(true);
            _canvasGroup.alpha = 0;
            _counterAnimator.ShowWithoutAnimation(level);

            _canvasGroup.DOFade(1, _showDuration);
            await UniTask.WaitForSeconds(_showDuration);
        }

        public async UniTask Hide()
        {
            await UniTask.WaitForSeconds(_hideDelay);
            _canvasGroup.DOFade(0, _hideDuration);
            await UniTask.WaitForSeconds(_hideDuration);
            _transitionScreen.SetActive(false);
        }
    }
}
