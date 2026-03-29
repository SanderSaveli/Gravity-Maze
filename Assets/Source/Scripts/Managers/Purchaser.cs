using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class Purchaser : MonoBehaviour
    {
        [SerializeField] private Button _removeAdsButton;
        [SerializeField] private Button _restorePurchases;
        [SerializeField] private TMP_Text _removeAdsPriceText;
        private const string REMOVE_ADS_ID = "com.SanderSaveli.GravityMaze.removeAds";
        private IIAPManager _iapService;

        [Inject]
        public void Construct(IIAPManager iapService)
        {
            _iapService = iapService;
        }

        private void OnEnable()
        {
            _removeAdsButton.onClick.AddListener(() => _iapService.BuyProduct(REMOVE_ADS_ID));
            _restorePurchases.onClick.AddListener(_iapService.RestorePurchases);
            _removeAdsPriceText.text = _iapService.GetLocalizedPrice(REMOVE_ADS_ID);
            _iapService.OnProductPriceUpdated += OnProductPriceUpdated;
        }

        private void OnDestroy()
        {
            _removeAdsButton.onClick.RemoveListener(() => _iapService.BuyProduct(REMOVE_ADS_ID));
            _restorePurchases.onClick.RemoveListener(_iapService.RestorePurchases);

            _iapService.OnProductPriceUpdated -= OnProductPriceUpdated;
        }

        private void OnProductPriceUpdated(string productId, string localizedPrice)
        {
            if (productId == REMOVE_ADS_ID)
                _removeAdsPriceText.text = localizedPrice;
        }
    }
}