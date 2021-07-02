using CoreBot.Models;
using System.Linq;

namespace CoreBot.CognitiveModels
{
    public partial class AddItemToTrolley
    {
        public TrolleyItem TrolleyItemX
        {
            get
            {
                var item = default(TrolleyItem);// Entities.TrolleyItem.FirstOrDefault().TrolleyItem;
                var productName = item.ProductName;
                var productQuantity = item.Quantity;
                var unitOfMeasure = item.UnitOfMeasure;

                return new TrolleyItem()
                {
                    ProductName = productName,
                    Quantity = productQuantity,
                    UnitOfMeasure = unitOfMeasure
                };
            }
        }
    }
}
