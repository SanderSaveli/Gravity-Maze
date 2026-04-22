using SanderSaveli.UDK.UI;
using System;
using System.Collections.Generic;

namespace SanderSaveli.GravityMaze
{
    public class ColorRadioGroup : RadioButtonGroup<ColorSheme>
    {
        private new void Start()
        {
            
        }

        public void SetButtons(List<RadioButton<ColorSheme>> buttons, ColorSheme selectedElement)
        {
            foreach (var radioButton in _radioButtons)
            {
                radioButton.OnSelectInput -= OnSelectInput;
            }

            _radioButtons = buttons;

            foreach (var radioButton in _radioButtons)
            {
                radioButton.Deselect();
                radioButton.OnSelectInput += OnSelectInput;
                if(radioButton.Value == selectedElement)
                {
                    _selectedElement = radioButton;
                }
            }
            base.Start();
        }

        internal void SetButtons(List<ColorSlot> colorSlots, ColorSheme selectedElement)
        {
            List< RadioButton < ColorSheme >> list = new List<RadioButton<ColorSheme>> ();

            foreach (var item in colorSlots)
            {
                list.Add(item);
            }
            SetButtons(list, selectedElement);
        }
    }
}
