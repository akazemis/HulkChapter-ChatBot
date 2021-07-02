using CoreBot.CognitiveModels;
using CoreBot.Repositories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Dialogs;
using Microsoft.BotBuilderSamples.Recognizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class AddItemToTrolleyDialog : CancelAndHelpDialog
    {
        const string LineBreak = "\r\n";
        private readonly IGlobalRepository _repository;
        private readonly GenericRecognizer _luisGenericRecognizer;

        public AddItemToTrolleyDialog(IGlobalRepository repository,
                                      GenericRecognizer luisGenericRecognizer) : base(nameof(AddItemToTrolleyDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                FinalStepAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
            _repository = repository;
            _luisGenericRecognizer = luisGenericRecognizer;
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisGenericRecognizer.RecognizeAsync<AddItemToTrolley>(stepContext.Context, cancellationToken);
            var promotionListMessage = await AddItemToTrolleyAndGetMessage("abc", luisResult);
            var messageText = $"Current Trolley:{LineBreak}{promotionListMessage}";
            var promptMessage = MessageFactory.Text(messageText, InputHints.IgnoringInput);
            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync();

        }

        private async Task<string> AddItemToTrolleyAndGetMessage(string userId, AddItemToTrolley addItemToTrolley)
        {
            var currentTrolleyItems = await _repository.GetTrolleyItems(userId);
            var trolleyItemToBeAdded = addItemToTrolley.TrolleyItemX;
            var products = await _repository.GetAllProducts();
            if(trolleyItemToBeAdded?.ProductName == null)
            {
                return "Item not found.";
            }
            var product = products.FirstOrDefault(x => x.ProductName.ToLower() == trolleyItemToBeAdded.ProductName.ToLower());
            if(product == null)
            {
                return "Item not found.";
            }
            var qty = 1m;
            if (string.IsNullOrWhiteSpace(addItemToTrolley?.Quantity))
            {
                qty = decimal.Parse(addItemToTrolley?.Quantity);
            }
            currentTrolleyItems.Add(new Models.TrolleyItem() 
            {
                ProductName = product.ProductName,
                Quantity = qty,
                UnitPrice = product.UnitPrice,
                ProductId = product.ProductId,
                UnitOfMeasure = trolleyItemToBeAdded?.UnitOfMeasure ?? product.UnitOfMeasure
            });
            return "Item added to trolley.";
        }
    }
}
