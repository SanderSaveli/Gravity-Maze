using CustomText;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class LanguageSlotView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private LanguageListElement _languageListElement;
        [SerializeField] private CustomText.CustomText _customText;

        [Header("Settings")]
        [SerializeField] private Custom_ColorStyle _activeColor;
        [SerializeField] private Custom_ColorStyle _pasiveColor;
        private void OnEnable()
        {
            _languageListElement.OnSelected += ChangeColor;
            ChangeColor(_languageListElement.IsSelected);
        }

        private void OnDisable()
        {
            _languageListElement.OnSelected -= ChangeColor;
        }

        private void ChangeColor(bool isActive)
        {
            Custom_ColorStyle color = isActive ? _activeColor : _pasiveColor;
            _customText.ChangeColor(color);
        }
    }
}
