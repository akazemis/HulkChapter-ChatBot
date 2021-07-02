using CoreBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Repositories
{
    public interface IGlobalRepository
    {
        Task<List<ProductPromotion>> GetProductPromotions(string pointOfTime);

        Task<List<TrolleyItem>> GetTrolleyItems(string userId);

        Task<List<Product>> GetAllProducts();

        Task AddTrolleyItem(string userId, TrolleyItem trolleyItem);

        Task RemoveTrolleyItem(string userId, string productName);
    }
}
