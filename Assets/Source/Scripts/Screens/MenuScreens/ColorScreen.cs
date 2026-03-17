using SanderSaveli.UDK.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class ColorScreen : UiScreen
    {
        [SerializeField] private ColorRadioGroup _radioGroup;
        [SerializeField] private List<ColorGroup> _colorGroups;
        [SerializeField] protected ColorGroupRadioGroup _colorGroup;
        private IColorManager _colorManager;
        private ColorGroup _activeGroup;

        [Inject]
        public void Construct(IColorManager colorManager)
        {
            _colorManager = colorManager;
        }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            _radioGroup.SetSelect(_colorManager.ActiveSheme.Value);
            _radioGroup.OnValueChanged += UpdateSelection;

            foreach (var colorGroup in _colorGroups)
            {
                if(colorGroup.HasColorInGroup(_colorManager.ActiveSheme.Value, out _)) 
                {
                    _activeGroup = colorGroup;
                    _colorGroup.SetSelect(colorGroup.Type);
                    colorGroup.Show();
                }
                else
                {
                    colorGroup.HideImmediately();
                }
            }

            _colorGroup.OnValueChanged += ChangeActiveGroup;
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _radioGroup.OnValueChanged -= UpdateSelection;
            _colorGroup.OnValueChanged -= ChangeActiveGroup;

        }

        private void UpdateSelection(ColorSheme colorSheme)
        {
            _colorManager.ActiveSheme.Value = colorSheme;
        }

        private void ChangeActiveGroup(ColorGroupType type)
        {
            if(_activeGroup == null || _activeGroup.Type != type)
            {
                _activeGroup.Hide();
                _activeGroup = _colorGroups.Find(c => c.Type == type);
                _activeGroup.Show();
            }
        }
    }
}
