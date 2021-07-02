using CoreBot.Models;
using System.Collections.Concurrent;
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

        ConcurrentDictionary<string, List<TrolleyItem>> _trolleysDictionary = new ConcurrentDictionary<string, List<TrolleyItem>>();

        public async Task<List<TrolleyItem>> GetTrolleyItems(string userId)
        {
            CreateTrolleyIfDoesNotExist(userId);
            return _trolleysDictionary[userId];
        }

        public async Task AddTrolleyItem(string userId, TrolleyItem trolleyItem)
        {
            CreateTrolleyIfDoesNotExist(userId);
            _trolleysDictionary[userId].Add(trolleyItem);
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

        public async Task RemoveTrolleyItem(string userId, string productName)
        {
            CreateTrolleyIfDoesNotExist(userId);
            var items = _trolleysDictionary[userId].FindAll(x => x.ProductName.ToLower() == productName.ToLower());
            var trolleyItems = _trolleysDictionary[userId];
            foreach (var item in items)
            {
                trolleyItems.Remove(item);
            }
        }

        private void CreateTrolleyIfDoesNotExist(string userId)
        {
            if (!_trolleysDictionary.ContainsKey(userId))
            {
                _trolleysDictionary[userId] = new List<TrolleyItem>();
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    ProductId = "1",
                    ProductName = "Carrot",
                    UnitOfMeasure = "pack",
                    UnitPrice = 1.5m,
                },
                new Product()
                {
                    ProductId = "2",
                    ProductName = "Potato Chips",
                    UnitOfMeasure = "pack",
                    UnitPrice = 1.5m,
                }
            };
        }
    }
}
