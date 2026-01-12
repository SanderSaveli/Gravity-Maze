using SanderSaveli.UDK.UI;

namespace SanderSaveli.GravityMaze
{
    public class ColorRadioGroup : RadioButtonGroup<ColorSheme>
    {
        public RadioButton<ColorSheme> ActiveElement => _selectedElement;
    }
}
