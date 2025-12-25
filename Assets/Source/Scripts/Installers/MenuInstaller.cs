using SanderSaveli.UDK.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<SignalInputOpenMenuScreen>();
            Container.DeclareSignal<SignalInputOpenMenuPopup>();
        }
    }
}
