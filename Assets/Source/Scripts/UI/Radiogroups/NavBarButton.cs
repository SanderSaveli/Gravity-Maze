using DG.Tweening;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class NavBarButton : RadioButton<NavBarOption>
    {
        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _jumpPart;
        [SerializeField] private Button _button;

        [Header("params")]
        [SerializeField] private Color _disableColor = Color.white;
        [SerializeField] private Color _enableColor = Color.white;
        [SerializeField] private float _colorChangeDuration = 0.2f;
        [Space]
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private float _jumpDuration = 0.4f;
        private Vector3 _jumpPos;

        private void Start()
        {
            _jumpPos = _jumpPart.anchoredPosition;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public override void Deselect()
        {
            _image.DOColor(_disableColor, _colorChangeDuration).SetLink(gameObject);
        }

        public override void Select()
        {
            _image.DOColor(_enableColor, _colorChangeDuration).SetLink(gameObject);
            _jumpPart.DOJumpAnchorPos(_jumpPos, _jumpHeight, 1, _jumpDuration).SetLink(gameObject);
        }

        private void HandleButtonClick()
        {
            OnSelectInput?.Invoke(this);
        }
    }
}
