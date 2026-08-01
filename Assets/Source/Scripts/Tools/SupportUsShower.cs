using R3;
using SanderSaveli.UDK;
using SanderSaveli.UDK.UI;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class SupportUsShower : MonoBehaviour
    {
        [SerializeField] private UiScreen _uiScreen;
        [SerializeField] private int _targetLevel;
        [SerializeField] private int _levelOffset;

        private const string SavePath = "Save/SupportUs";

        private ILevelStorage _levelManager;
        private CompositeDisposable _disposable;
        private IStorageService _storageService;
        private SupportUsData _data;
        private bool _isLoaded;

        [Inject]
        public void Construct(ILevelStorage levelManager)
        {
            _levelManager = levelManager;
            _storageService = new EncryptedJsonToFileStorageService();
        }

        private void Start()
        {
            _storageService.Load<SupportUsData>(SavePath, HandleLoaded);
        }

        private void OnEnable()
        {
            if (!_isLoaded)
                return;

            StartLevelCheck();
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public void ShowComplete()
        {
            _data.isShown = true;
            _storageService.Save(SavePath, _data);
        }

        public void MaybeLater()
        {
            _data.isShown = false;
            _data.levelOffset += _levelOffset;
            _storageService.Save(SavePath, _data);
        }

        private void IsShowScreen(int level)
        {
            Debug.Log("Level " + level + " target Level " + _targetLevel + " level offset "+ _data.levelOffset);
            if (_data.isShown) return;

            if (level >= _targetLevel + _data.levelOffset)
            {
                _uiScreen.Show();
            }
        }

        private void HandleLoaded(SupportUsData data)
        {
            _data = data;
            _isLoaded = true;

            if (!isActiveAndEnabled)
                return;

            StartLevelCheck();
        }

        private void StartLevelCheck()
        {
            SubscribeToLevelUpdates();
        }

        private void SubscribeToLevelUpdates()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _levelManager.CurrentLevel.Subscribe(IsShowScreen).AddTo(_disposable);
        }
    }
}
