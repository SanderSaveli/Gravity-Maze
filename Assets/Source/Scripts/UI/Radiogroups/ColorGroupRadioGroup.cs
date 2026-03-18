using SanderSaveli.UDK.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorGroupRadioGroup : RadioButtonGroup<ColorGroupType>
    {
        public RadioButton<ColorGroupType> ActiveElement => _selectedElement;
    }
}
