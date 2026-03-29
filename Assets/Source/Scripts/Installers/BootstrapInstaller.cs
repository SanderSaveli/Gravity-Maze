using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private DataManager _dataManager;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private SOBasedGameplayConfig _gameplayConfig;
        [SerializeField] private FirebaseInitializer _analyticManager;
        [SerializeField] private VibrationManager _vibrationManager;
        [SerializeField] private AppSettings _appSettings;
        [SerializeField] private LockalTextManager _textManager;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private ColorManager _colorManager;
        [SerializeField] private AdManager _adManager;
        [SerializeField] private TimeManager _timeManager;

        private GameContext _gameContext;
        public override void InstallBindings()
        {
            _gameContext = new GameContext();
            SignalBusInstaller.Install(Container);
            Container.Bind<ILevelManager>().FromInstance(_levelManager).AsSingle().NonLazy();
            Container.Bind<IGameContext>().FromInstance(_gameContext).AsSingle().NonLazy();
            Container.Bind<IGameplayConfig>().FromInstance(_gameplayConfig).AsSingle().NonLazy();
            Container.Bind<ILevelStorage>().FromInstance(_dataManager.LevelStorage).AsSingle().NonLazy();
            Container.Bind<IAnalyticManager>().FromInstance(_analyticManager).AsSingle().NonLazy();
            Container.Bind<IVibrationManager>().FromInstance(_vibrationManager).AsSingle().NonLazy();
            Container.Bind<IAppSettings>().FromInstance(_appSettings).AsSingle().NonLazy();
            Container.Bind<ITextManager>().FromInstance(_textManager).AsSingle().NonLazy();
            Container.Bind<ILanguageChanger<Language>>().FromInstance(_textManager).AsSingle().NonLazy();
            Container.Bind<IAudioManager>().FromInstance(_audioManager).AsSingle().NonLazy();
            Container.Bind<IColorManager>().FromInstance(_colorManager).AsSingle().NonLazy();
            Container.Bind<IAdManager>().FromInstance(_adManager).AsSingle().NonLazy();
            Container.Bind<ITimeManager>().FromInstance(_timeManager).AsSingle().NonLazy();

            #region Signals
            Container.DeclareSignal<SignalInputAction>();
            Container.DeclareSignal<SignalInputClosePopup>();
            Container.DeclareSignal<SignalInputCloseScreen>();
            #endregion
        }
    }
}
