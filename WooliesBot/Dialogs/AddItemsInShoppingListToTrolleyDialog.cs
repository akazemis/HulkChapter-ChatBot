using CoreBot.CognitiveModels;
using CoreBot.Repositories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Dialogs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Models;

namespace CoreBot.Dialogs
{
    public class AddItemsInShoppingListToTrolleyDialog : CancelAndHelpDialog
    {
        const string LineBreak = "\r\n";
        private readonly IGlobalRepository _repository;

        public AddItemsInShoppingListToTrolleyDialog(IGlobalRepository repository) : base(nameof(AddItemsInShoppingListToTrolleyDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                AddItemsStepAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
            _repository = repository;
        }

        private async Task<DialogTurnResult> AddItemsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var addItems = (AddItemsInShoppingListToTrolley)stepContext.Options;
            var messageText = $"These products are added to trolley:{LineBreak}" + await AddListToTrolley();

            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync(addItems, cancellationToken);

        }

        private async Task<string> AddListToTrolley()
        {
            var products = await _repository.GetShoppingList("Now");
            
            foreach (var product in products)
            {
                await _repository.AddTrolleyItem("abc", product);
            }   
            
            var result = string.Join(LineBreak,
                products.Select(p => $"{p.ProductName} ({p.Quantity} x ${p.UnitPrice})"));
            return result;
        }
    }
}
