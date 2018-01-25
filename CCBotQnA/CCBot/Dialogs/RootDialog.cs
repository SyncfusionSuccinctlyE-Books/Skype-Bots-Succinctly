using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;

namespace CCBot.Dialogs
{
    [Serializable]
    [QnAMakerService("7f223def851a454e878402d977708c30", "ba929905-ce12-4f56-81d6-2c2969633449")]
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

    [Serializable]
    [QnAMakerService("7f223def851a454e878402d977708c30", "cb34ee1c-f1dd-415e-87b8-0d863c585956")]
    public class FrenchDialog : QnAMakerDialog<object>
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