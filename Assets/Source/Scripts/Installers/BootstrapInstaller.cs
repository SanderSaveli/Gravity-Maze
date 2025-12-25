using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private SOBasedGameplayConfig _gameplayConfig;

        private GameContext _gameContext;
        public override void InstallBindings()
        {
            _gameContext = new GameContext();
            SignalBusInstaller.Install(Container);
            Container.Bind<ILevelManager>().FromInstance(_levelManager).AsSingle().NonLazy();
            Container.Bind<IGameContext>().FromInstance(_gameContext).AsSingle().NonLazy();
            Container.Bind<IGameplayConfig>().FromInstance(_gameplayConfig).AsSingle().NonLazy();

            #region Signals
            Container.DeclareSignal<SignalInputAction>();
            Container.DeclareSignal<SignalInputClosePopup>();
            Container.DeclareSignal<SignalInputCloseScreen>();
            #endregion
        }
    }
}
