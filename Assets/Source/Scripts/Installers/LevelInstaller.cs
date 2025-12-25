using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ILevelManager levelManager = Container.Resolve<ILevelManager>();
            IGameContext gameContext = Container.Resolve<IGameContext>();
            ILevelProvider levelProvider = levelManager.Levels[gameContext.LevelNumber];
            ILevelProvider instance = Container.InstantiatePrefabForComponent<ILevelProvider>(levelProvider as Object);
            Container.Bind<ILevelProvider>().FromInstance(instance).AsSingle().NonLazy();
        }
    }
}
