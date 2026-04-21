using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class IconAnimationController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private SelectButton _button;
        [SerializeField] private LottieAnimator _animator;

        [Header("Params")]
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;
        private bool _lastStatus;

        private void OnEnable()
        {
            _button.OnSwitched += OnStatusChange;
            _animator.OnStopPlay += ApplySprite;
            _lastStatus = _button.IsSelected;
            ApplySprite();
        }

        private void OnDisable()
        {
            _button.OnSwitched -= OnStatusChange;
            _animator.OnStopPlay += ApplySprite;
        }

        private void OnStatusChange(bool status)
        {
            _image.gameObject.SetActive(false);
            _animator.gameObject.SetActive(true);
            _lastStatus = status;
            if (status)
            {
                _animator.Select();
            }
            else
            {
                _animator.Deselect();
            }
        }

        private void ApplySprite()
        {
            if (_lastStatus)
            {
                _image.sprite = _onSprite;
            }
            else
            {
                _image.sprite = _offSprite;
            }
            _image.gameObject.SetActive(true);
            _animator.gameObject.SetActive(false);
        }
    }
}
