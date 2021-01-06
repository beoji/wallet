using System.Collections.Generic;

namespace Wallet
{
    interface IManager
    {
        void Load();
        void Save();
        void AddPurchace(Purchace purchace);
        Category GetOrCreateCategory(string categoryName);
        IEnumerable<Purchace> GetPurchaces();
        IEnumerable<Category> GetCategories();
    }
}
