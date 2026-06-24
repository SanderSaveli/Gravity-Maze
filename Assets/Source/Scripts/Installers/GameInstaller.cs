using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private SlowingRotationManager _rotationManager;
        [SerializeField] private StarManager _starManager;

        public override void InstallBindings()
        {
            Container.Bind<IInputManager>().FromInstance(_inputManager).AsSingle().NonLazy();
            Container.Bind<IRotationManager>().FromInstance(_rotationManager).AsSingle().NonLazy();
            Container.Bind<IStarManager>().FromInstance(_starManager).AsSingle().NonLazy();

            Container.DeclareSignal<SignalPlayerExitContour>();
            Container.DeclareSignal<SignalStarCollected>();
        }
    }

}