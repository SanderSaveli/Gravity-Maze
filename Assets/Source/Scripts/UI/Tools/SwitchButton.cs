using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class SwitchButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private LottieAnimator _animator;

        private bool isActive;

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleSwitch);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleSwitch);
        }

        private void HandleSwitch()
        {
            isActive = !isActive;
            if (isActive)
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
