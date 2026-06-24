using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroup : MonoBehaviour, ISelectable
    {
        public ColorGroupType Type => _colorGroupType;
        public List<ColorSlot> ColorSlots => _colorsInGroup;

        public bool IsSelected { get; private set; }

        [SerializeField] private SelectingSnapScroll _snapScroll;
        [SerializeField] private ColorFiller _colorFiller;
        [SerializeField] private Transform _colorParent;
        [SerializeField] private RectTransform _selfTransform;
        [SerializeField] private ColorGroupRadioGroup _radioGroup;

        [Header("Params")]
        [SerializeField] private ColorGroupType _colorGroupType;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;
        private IColorManager _colorManager;
        private List<ColorSlot> _colorsInGroup;

        [Inject]
        public void Construct(IColorManager colorManager)
        {
            _colorManager = colorManager;
        }

        public bool HasColorInGroup(ColorSheme color, out ColorRadioButton colorRadioButton)
        {
            colorRadioButton = _colorsInGroup.FirstOrDefault(x => x.Value == color);
            return colorRadioButton != null;
        }

        public void Init()
        {
            switch (Type)
            {
                case ColorGroupType.Light:
                    _colorsInGroup = _colorFiller.Fill(_colorManager.LightColors.ToList(), _colorParent);
                    break;
                case ColorGroupType.Dark:
                    _colorsInGroup = _colorFiller.Fill(_colorManager.DarkColors.ToList(), _colorParent);
                    break;
                case ColorGroupType.Multicolor:
                    _colorsInGroup = _colorFiller.Fill(_colorManager.MulticolorColors.ToList(), _colorParent);
                    break;
                default:
                    break;
            }
        }

        public void Select()
        {
            IsSelected = true;
            if (_radioGroup.Value != Type)
            {
                _radioGroup.SetSelect(Type);
            }
            else if (!_snapScroll.IsSpapping)
            {
                _snapScroll.SnapTo(_selfTransform);
            }
        }

        public void Deselect()
        {
            IsSelected = false;
        }
    }
}
