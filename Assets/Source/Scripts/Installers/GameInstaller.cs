using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private RotationManager _rotationManager;
        [SerializeField] private LevelProvider _levelProvider;

        public override void InstallBindings()
        {
            Container.Bind<IInputManager>().FromInstance(_inputManager).AsSingle().NonLazy();
            Container.Bind<IRotationManager>().FromInstance(_rotationManager).AsSingle().NonLazy();
            Container.Bind<ILevelProvider>().FromInstance(_levelProvider).AsSingle().NonLazy();
        }
    }

}