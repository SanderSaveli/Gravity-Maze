using SanderSaveli.UDK.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroup : MonoBehaviour
    {
        public ColorGroupType Type => _colorGroupType;

        [SerializeField] private ShowHideAnimation _leftShowHideAnimation;
        [SerializeField] private List<ColorRadioButton> _colorsInGroup;

        [Header("Params")]
        [SerializeField] private ColorGroupType _colorGroupType;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;

        public bool HasColorInGroup(ColorSheme color, out ColorRadioButton colorRadioButton)
        {
            colorRadioButton = _colorsInGroup.FirstOrDefault(x => x.Value == color);
            return colorRadioButton != null;
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
