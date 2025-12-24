using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ILevelManager levelManager = Container.Resolve<ILevelManager>();
            ILevelProvider levelProvider = levelManager.GetCurrentLevel();
            ILevelProvider instance = Container.InstantiatePrefabForComponent<ILevelProvider>(levelProvider as Object);
            Container.Bind<ILevelProvider>().FromInstance(instance).AsSingle().NonLazy();
        }
    }
}
