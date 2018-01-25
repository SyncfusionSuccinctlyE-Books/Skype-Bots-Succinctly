using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using CakeBotSuccinctly.Models;

namespace CakeBotSuccinctly.Dialogs
{
    [Serializable]
    public class WelcomeDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(Str.cStrHi);
            await Respond(context);

            context.Wait(MessageReceivedAsync);
        }

        private static async Task Respond(IDialogContext context)
        {
            var userName = string.Empty;
            context.UserData.TryGetValue(Str.cStrName, out userName);

            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync(Str.cStrNameQ);
                context.UserData.SetValue(Str.cStrGetName, true);
            }
            else
            {
                await context.PostAsync(string.Format("Hi {0}, how may I help you today? You may order...", userName));
                await context.PostAsync(string.Join(", ", Str.CakeTypes));
            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var msg = await argument;
            string userName = string.Empty;
            bool getName = false;

            context.UserData.TryGetValue(Str.cStrName, out userName);
            context.UserData.TryGetValue(Str.cStrGetName, out getName);

            if (getName)
            {
                userName = msg.Text;
                context.UserData.SetValue(Str.cStrName, userName);
                context.UserData.SetValue(Str.cStrGetName, false);
            }

            await Respond(context);
            context.Done(msg);
        }
    }
}