using R3;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class WaveSpawnerActiveColorSetter : MonoBehaviour
    {
        [SerializeField] private WaveSpawner _waveSpawner;
        private IColorManager _colorManager;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public void Construct(IColorManager colorManager)
        {
            _colorManager = colorManager;
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _colorManager.ActiveSheme.Subscribe(UpdateSheme).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void UpdateSheme(ColorSheme colorSheme)
        {
            _waveSpawner.SetWaveColor(_colorManager.GetActiveColorOfSheme(colorSheme));
        }
    }
}
