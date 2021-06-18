using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreBot.CognitiveModels
{
    public class GetShoppingList : IRecognizerConvert
    {
        public string Text;
        public string AlteredText;

        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            public class _Instance
            {
                public InstanceData[] PointOfTime;
            }

            [JsonProperty("$instance")]
            public _Instance _instance;

            public string[] PointOfTime;
        }
        
        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties { get; set; }

        public string PointOfTime { get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<GetShoppingList>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }
    }
}
