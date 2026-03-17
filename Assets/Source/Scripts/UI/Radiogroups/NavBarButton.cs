using CustomText;
using DG.Tweening;
using SanderSaveli.UDK.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.GravityMaze
{
    public class NavBarButton : RadioButton<NavBarOption>
    {
        [Header("Components")]
        [SerializeField] private ImageColorByType _iconColorByType;
        [SerializeField] private ImageColorByType _bgColorByType;
        [SerializeField] private RectTransform _jumpPart;
        [SerializeField] private Button _button;

        [Header("params")]
        [SerializeField] private Custom_ColorStyle _disableIconColor;
        [SerializeField] private Custom_ColorStyle _enableIconColor;
        [SerializeField] private Custom_ColorStyle _disableBGColor;
        [SerializeField] private Custom_ColorStyle _enableBGColor;

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
            _iconColorByType.ChangeColorWithAnimation(_disableIconColor, _colorChangeDuration);
            _bgColorByType.ChangeColorWithAnimation(_disableBGColor, _colorChangeDuration);
        }

        public override void Select()
        {
            _iconColorByType.ChangeColorWithAnimation(_enableIconColor, _colorChangeDuration);
            _bgColorByType.ChangeColorWithAnimation(_enableBGColor, _colorChangeDuration);

            _jumpPart.DOJumpAnchorPos(_jumpPos, _jumpHeight, 1, _jumpDuration).SetLink(gameObject);
        }

        private void HandleButtonClick()
        {
            OnSelectInput?.Invoke(this);
        }
    }
}
