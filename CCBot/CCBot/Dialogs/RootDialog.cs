using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;

namespace CCBot.Dialogs
{
    [Serializable]
    [QnAMakerService("SUBSCRIPTION_KEY", "KNOWLEDGE_BASE_ID")]
    public class RootDialog : QnAMakerDialog<object>
    {
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, couldn't find an answer for '{originalQueryText}'.");
            context.Wait(MessageReceived);
        }

        [QnAMakerResponseHandler(50)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            await context.PostAsync($"Found an answer that could help...{result.Answer}.");
            context.Wait(MessageReceived);
        }
    }
}