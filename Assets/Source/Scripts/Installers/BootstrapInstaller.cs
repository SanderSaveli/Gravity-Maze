using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LevelManager _levelManager;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<ILevelManager>().FromInstance(_levelManager).AsSingle().NonLazy();

            #region Signals
            Container.DeclareSignal<SignalInputAction>();
            Container.DeclareSignal<SignalInputClosePopup>();
            Container.DeclareSignal<SignalInputCloseScreen>();
            #endregion
        }
    }
}
