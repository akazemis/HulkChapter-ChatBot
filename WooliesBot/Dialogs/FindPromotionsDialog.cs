using CoreBot.CognitiveModels;
using CoreBot.Repositories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Dialogs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class FindPromotionsDialog : CancelAndHelpDialog
    {
        const string LineBreak = "\r\n";
        private readonly IGlobalRepository _repository;

        public FindPromotionsDialog(IGlobalRepository repository) : base(nameof(FindPromotionsDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                PointOfTimeStepAsync,
                FinalStepAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
            _repository = repository;
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
            var messageText = $"These products are on promotion {promotionDetails.PointOfTime}:{LineBreak}" + await GetProductPromotions(promotionDetails.PointOfTime);

            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync(promotionDetails, cancellationToken);

        }

        private async Task<string> GetProductPromotions(string pointOfTime)
        {
            var productPromotions = await _repository.GetProductPromotions(pointOfTime);
            var result = string.Join(LineBreak,
                                     productPromotions.Select(p => $"{p.ProductTitle} ({p.PromotionDescription})"));
            return result;
        }
    }
}
