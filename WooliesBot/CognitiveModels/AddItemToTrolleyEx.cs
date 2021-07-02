using System.Linq;

namespace CoreBot.CognitiveModels
{
    public partial class AddItemToTrolley
    {
        public TrolleyItem TrolleyItemX
        {
            get
            {
                var item = Entities.TrolleyItem.FirstOrDefault().TrolleyItem;
                var productName = item.ProductName;
                var productQuantity = item.ProductQuantity;
                var unitOfMeasure = item.UnitOfMeasure;

                return new TrolleyItem()
                {
                    ProductName = productName,
                    ProductQuantity = productQuantity,
                    UnitOfMeasure = unitOfMeasure
                };
            }
        }
    }
}
