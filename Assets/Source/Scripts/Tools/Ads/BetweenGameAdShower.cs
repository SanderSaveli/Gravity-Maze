using Cysharp.Threading.Tasks;
using R3;
using SanderSaveli.UDK;
using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class BetweenGameAdShower : MonoBehaviour
    {
        [SerializeField] private float _timeBetweenAds;
        [SerializeField] private int _attemtsBetweenAds;

        private const string AdShowtimeKey = "AdShowTime";

        private int _gamesWithoutAd;
        private IAdManager _adManager;
        private TimerHandle? _timer;
        private bool _isTimerRing;
        private bool _isAdRemoved;

        private IAppSettings _appSettings;
        private CompositeDisposable _compositeDisposable;

        [Inject]
        public void Construct(IAdManager adManager, IAppSettings appSettings)
        {
            _adManager = adManager;
            _appSettings = appSettings;
        }

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            _appSettings.IsAdsRemoved.Subscribe(ChangeAdStatus).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void Start()
        {
            LoadGamesWithoutAd();
            _timer = Timer.StartTimerRealtime(_timeBetweenAds, OnTimerRing);
        }

        private void ChangeAdStatus(bool isAdRemoved) => _isAdRemoved = isAdRemoved;

        public async UniTask ShowAdIfNeeded()
        {
            if (_isAdRemoved)
            {
                Debug.Log("Ad does not show because it's removed");
                return;
            }
            _gamesWithoutAd++;

            if (_gamesWithoutAd >= _attemtsBetweenAds || _isTimerRing)
            {
                IAdAdapter adAdapter = _adManager.ShowBetweenScreenAd();

                if (adAdapter != null)
                {
                    await WaitForAd(adAdapter);
                }

                _gamesWithoutAd = 0;
                _timer?.Cancel();
                _timer = Timer.StartTimerRealtime(_timeBetweenAds, OnTimerRing);
            }

            PlayerPrefs.SetInt(AdShowtimeKey, _gamesWithoutAd);
            await UniTask.Yield();
        }

        private void LoadGamesWithoutAd()
        {
            if (PlayerPrefs.HasKey(AdShowtimeKey))
            {
                _gamesWithoutAd = PlayerPrefs.GetInt(AdShowtimeKey);
            }
            else
            {
                _gamesWithoutAd = 0;
                PlayerPrefs.SetInt(AdShowtimeKey, 0);
            }
        }

        private void OnTimerRing()
        {
            _isTimerRing = true;
            _timer = null;
        }

        private UniTask WaitForAd(IAdAdapter adapter)
        {
            if (adapter == null)
                return UniTask.CompletedTask;

            var tcs = new UniTaskCompletionSource();

            adapter.OnEndShow += () =>
            {
                tcs.TrySetResult();
            };

            return tcs.Task;
        }
    }
}
