using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using System.Threading.Tasks;

namespace SanderSaveli.GravityMaze
{
    public class IAPManager : MonoBehaviour, IIAPManager
    {
        private StoreController _storeController;
        private Dictionary<string, Product> _productsDict = new Dictionary<string, Product>();

        public event Action<string, string> OnProductPriceUpdated;

        private async void Start()
        {
            await InitializeIAP();
        }

        private async Task InitializeIAP()
        {
            await UnityServices.InitializeAsync();

            _storeController = UnityIAPServices.StoreController();

            _storeController.OnProductsFetched += OnProductsFetched;
            _storeController.OnPurchasesFetched += OnPurchasesFetched;
            _storeController.OnPurchasePending += OnPurchasePending;
            _storeController.OnPurchaseFailed += OnPurchaseFailed;

            await _storeController.Connect();

            var products = new List<ProductDefinition>
            {
                new ProductDefinition("com.SanderSaveli.GravityMaze.removeAds", ProductType.NonConsumable)
            };

            _storeController.FetchProducts(products);
        }

        private void OnProductsFetched(List<Product> products)
        {
            _productsDict.Clear();

            foreach (var product in products)
            {
                _productsDict[product.definition.id] = product;

                OnProductPriceUpdated?.Invoke(product.definition.id, product.metadata.localizedPriceString);
            }

            _storeController.FetchPurchases();
        }

        private void OnPurchasesFetched(Orders orders)
        {
            foreach (var order in orders.ConfirmedOrders)
            {
                foreach (var item in order.CartOrdered.Items())
                {
                    ProcessProduct(item.Product.definition.id);
                }
            }
        }

        private void OnPurchasePending(PendingOrder order)
        {
            foreach (var item in order.CartOrdered.Items())
            {
                ProcessProduct(item.Product.definition.id);
            }

            _storeController.ConfirmPurchase(order);
        }

        private void OnPurchaseFailed(FailedOrder failedOrder)
        {
            foreach (var item in failedOrder.CartOrdered.Items())
            {
                Debug.LogError($"Purchase failed: {item.Product.definition.id} | {failedOrder.FailureReason}");
            }
        }

        private void ProcessProduct(string productId)
        {
            if (productId == "com.SanderSaveli.GravityMaze.removeAds")
            {
                Debug.Log("Ads removed!");
            }
        }

        public void BuyProduct(string productId)
        {
            if (_storeController == null) return;
            _storeController.PurchaseProduct(productId);
        }

        public void RestorePurchases()
        {
            if (_storeController == null) return;
            _storeController.FetchPurchases();
        }

        public string GetLocalizedPrice(string productId)
        {
            if (_productsDict.TryGetValue(productId, out var product))
                return product.metadata.localizedPriceString;

            return null;
        }
    }
}