using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class IconAnimationController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SelectButton _button;
        [SerializeField] private LottieAnimator _animator;

        [Header("Params")]
        [SerializeField] private Sprite _sprite;

        private void OnEnable()
        {
            _button.OnSwitched += OnStatusChange;
        }

        private void OnDisable()
        {
            _button.OnSwitched -= OnStatusChange;
        }

        private void OnStatusChange(bool status)
        {
            if (status)
            {
                _animator.Select();
            }
            else
            {
                _animator.Deselect();
            }
        }
    }
}
