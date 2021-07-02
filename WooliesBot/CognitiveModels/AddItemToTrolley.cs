using CoreBot.Models;
using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using System.Linq;

namespace CoreBot.CognitiveModels
{
    public partial class AddItemToTrolley: IRecognizerConvert
    {
        public string Quantity { get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<FindPromotions>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            // TODO: Set properties
        }
    }
}