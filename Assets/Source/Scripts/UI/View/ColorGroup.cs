using CustomText;
using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroup : MonoBehaviour
    {
        public ColorGroupType Type => _colorGroupType;
        public List<ColorSlot> ColorSlots => _colorsInGroup;

        [SerializeField] private ShowHideAnimation _leftShowHideAnimation;
        [SerializeField] private ColorFiller _colorFiller;
        [SerializeField] private Transform _colorParent;

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

        public void Show()
        {
            gameObject.SetActive(true);
            _leftShowHideAnimation.Show(0, _showDuration, null);
        }

        public void Hide()
        {
            _leftShowHideAnimation.Hide(0, _hideDuration, () => gameObject.SetActive(false));
        }

        public void ShowImmediately()
        {
            gameObject.SetActive(true);
            _leftShowHideAnimation.ShowImmediately();
        }

        public void HideImmediately()
        {
            _leftShowHideAnimation.HideImmediately();
            gameObject.SetActive(false);
        }
    }
}
