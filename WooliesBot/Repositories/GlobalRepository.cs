using CoreBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Repositories
{
    public class GlobalRepository : IGlobalRepository
    {
        public async Task<List<ProductPromotion>> GetProductPromotions(string pointOfTime)
        {
            return new List<ProductPromotion>()
            {
                new ProductPromotion()
                {
                    ProductId = "1",
                    ProductTitle = "Carrot",
                    PromotionDescription = "Buy 1 get 1 free"
                },
                new ProductPromotion()
                {
                    ProductId = "2",
                    ProductTitle = "Potato Chips",
                    PromotionDescription = "Buy 2 get 1 free"
                }
            };
        }

        public async Task<List<Product>> GetShoppingList(string pointOfTime)
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "1",
                    Title = "Lor Coffee",
                    Description = "Lor Coffee Pod"
                },
                new Product()
                {
                    Id = "2",
                    Title = "Potato Chips",
                    Description = "Smiths Potato Chip"
                }
            };
        }
    }
}
