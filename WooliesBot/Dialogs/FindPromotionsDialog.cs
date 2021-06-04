using CoreBot.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class FindPromotionsDialog : CancelAndHelpDialog
    {
        public FindPromotionsDialog() : base(nameof(FindPromotionsDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                PointOfTimeStepAsync,
                FinalStepAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
        }
        private async Task<DialogTurnResult> PointOfTimeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promotionDetails = (FindPromotions)stepContext.Options;

            if (promotionDetails.PointOfTime == null)
            {
                promotionDetails.PointOfTime = "Now";
            }

            return await stepContext.NextAsync(promotionDetails, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var promotionDetails = (FindPromotions)stepContext.Options;
            var messageText = $"These products are on promotion {promotionDetails.PointOfTime}";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync(promotionDetails, cancellationToken);

        }
    }
}
