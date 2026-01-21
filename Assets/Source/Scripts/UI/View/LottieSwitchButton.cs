using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class LottieSwitchButton : SelectButton
    {
        [SerializeField] private LottieAnimator _animator;

        protected override void HandleDeselect()
        {
            _animator.Deselect();
        }

        protected override void HandleSelect()
        {
            _animator.Select();
        }
    }
}
