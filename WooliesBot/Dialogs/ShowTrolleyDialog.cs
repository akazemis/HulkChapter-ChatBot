using CoreBot.Repositories;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.BotBuilderSamples.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class ShowTrolleyDialog : CancelAndHelpDialog
    {
        const string LineBreak = "\r\n";
        private readonly IGlobalRepository _repository;

        public ShowTrolleyDialog(IGlobalRepository repository) : base(nameof(ShowTrolleyDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                FinalStepAsync
            }));
            InitialDialogId = nameof(WaterfallDialog);
            _repository = repository;
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promotionListMessage = await GetTrolleyItemsString("abc");
            var messageText = $"Current Trolley:{LineBreak}{promotionListMessage}";
            var promptMessage = MessageFactory.Text(messageText, InputHints.IgnoringInput);
            await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync();

        }

        private async Task<string> GetTrolleyItemsString(string userId)
        {
            var trolleyItems = await _repository.GetTrolleyItems(userId);
            if (!trolleyItems.Any())
            {
                return "Empty";
            }
            var result = string.Join(LineBreak,
                                     trolleyItems.Select(item => $"<img src='{item.PhotoUrl}' />{item.Quantity} x {item.ProductName} (Cost = ${item.TotalPrice})"));
            return result;
        }
    }
}
