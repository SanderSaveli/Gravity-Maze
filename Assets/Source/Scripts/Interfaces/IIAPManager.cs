using System;

namespace SanderSaveli.GravityMaze
{
    public interface IIAPManager
    {
        void BuyProduct(string productId);

        void RestorePurchases();

        string GetLocalizedPrice(string productId);

        event Action<string, string> OnProductPriceUpdated;
    }
}