using SanderSaveli.UDK.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroup : MonoBehaviour
    {
        public ColorGroupType Type => _colorGroupType;

        [SerializeField] private ShowHideAnimation _showHideAnimation;
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
            _showHideAnimation.Show(0, _showDuration, null);
        }

        public void Hide()
        {
            _showHideAnimation.Hide(0, _hideDuration, () => gameObject.SetActive(false));
        }

        public void ShowImmediately()
        {
            gameObject.SetActive(true);
            _showHideAnimation.ShowImmediately();
        }

        public void HideImmediately()
        {
            _showHideAnimation.HideImmediately();
            gameObject.SetActive(false);
        }
    }
}
