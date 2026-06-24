using R3;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class NavBarsColorWaveView : MonoBehaviour
    {
        [SerializeField] private WaveSpawner _spawner;

        private INotifficationManager _notifficationManager;
        private CompositeDisposable _compositeDisposable;
        private IColorManager _colorManager;

        [Inject]
        public void Construct(INotifficationManager notifficationManager, IColorManager colorManager)
        {
            _notifficationManager = notifficationManager;
            _colorManager = colorManager;
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _notifficationManager.HasUnshownColors.Subscribe(ChangeWaveStatus).AddTo(_compositeDisposable);
            _colorManager.ActiveSheme.Subscribe(UpdateSheme).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void ChangeWaveStatus(bool isActive)
        {
            if (isActive)
            {
                _spawner.StartSpawn();
            }
            else
            {
                _spawner.StopSpawn();
            }
        }

        private void UpdateSheme(ColorSheme colorSheme)
        {
            _spawner.SetWaveColor(_colorManager.GetActiveColorOfSheme(colorSheme));
        }
    }
}
