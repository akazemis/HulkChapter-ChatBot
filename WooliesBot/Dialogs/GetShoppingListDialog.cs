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
    public class GetShoppingListDialog : CancelAndHelpDialog
    {
        const string LineBreak = "\r\n";
        private readonly IGlobalRepository _repository;

        public GetShoppingListDialog(IGlobalRepository repository) : base(nameof(GetShoppingListDialog))
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
            var shoppingList = (GetShoppingList)stepContext.Options;

            if (shoppingList.PointOfTime == null)
            {
                shoppingList.PointOfTime = "Now";
            }

            return await stepContext.NextAsync(shoppingList, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var shoppingList = (GetShoppingList)stepContext.Options;
            var messageText = $"These products are in your shopping list {shoppingList.PointOfTime}:{LineBreak}" + await GetShoppingList(shoppingList.PointOfTime);

            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync(shoppingList, cancellationToken);

        }

        private async Task<string> GetShoppingList(string pointOfTime)
        {
            var products = await _repository.GetShoppingList(pointOfTime);
            var result = string.Join(LineBreak,
                products.Select(p => $"{p.Title} ({p.Description})"));
            return result;
        }
    }
}
