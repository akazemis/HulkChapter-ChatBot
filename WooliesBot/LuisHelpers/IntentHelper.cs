using CoreBot.CognitiveModels;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.LuisHelpers
{
    public class IntentHelper
    {
        public static async Task<Intent> GetIntent(IRecognizer recognizer, ITurnContext context, CancellationToken cancellationToken)
        {
            var recognitionResult = await recognizer.RecognizeAsync(context, cancellationToken);
            var topIntent = recognitionResult.GetTopScoringIntent();
            var intent = Enum.Parse<Intent>(topIntent.intent);
            return intent;
        }
    }
}
