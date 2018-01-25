using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using CCBot.Dialogs;
using System.Threading;

public class FaqSettingsDialog : IDialog<object>
{
    public async Task StartAsync(IDialogContext context)
    {
        await context.PostAsync("French FAQ. Type 'english' to go back.");

        context.Wait(MessageReceived);
    }

    private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
    {
        var message = await result;

        if ((message.Text != null) && (message.Text.Trim().Length > 0))
        {
            if (message.Text.ToLower().Contains("english"))
                context.Done<object>(null);
            else
            {
                var fDialog = new FrenchDialog();
                await context.Forward(fDialog, null, message, CancellationToken.None);
            }
        }
        else
        {
            context.Fail(new Exception("Message was not a string or was an empty string."));
        }
    }
}