using SanderSaveli.UDK.UI;

namespace SanderSaveli.GravityMaze
{
    public class NavBarRadioGroup : RadioButtonGroup<NavBarOption>
    {   
        public RadioButton<NavBarOption> ActiveElement => _selectedElement;
    }
}
