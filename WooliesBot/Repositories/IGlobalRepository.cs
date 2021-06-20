using CoreBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Repositories
{
    public interface IGlobalRepository
    {
        Task<List<ProductPromotion>> GetProductPromotions(string pointOfTime);
        Task<List<Product>> GetShoppingList(string pointOfTime);
    }
}
