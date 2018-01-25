using CakeBotSuccinctly.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace CakeBotSuccinctly.Dialogs
{
    [LuisModel("App Key", "Api Key")]
    [Serializable]
    public class LuisDialog : LuisDialog<Cakes>
    {
        private readonly BuildFormDelegate<Cakes> OrderCake;

        [field: NonSerialized()]
        protected Activity _msg;

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            _msg = (Activity)await item;
            await base.MessageReceived(context, item);
        }

        public LuisDialog(BuildFormDelegate<Cakes> orderCake)
        {
            OrderCake = orderCake;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult res)
        {
            await context.PostAsync(Str.cStrDontUnderstand);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Welcome")]
        public async Task Welcome(IDialogContext context, LuisResult res)
        {
            await Task.Run(() => context.Call(new WelcomeDialog(), Callback));
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            await Task.Run(() => context.Wait(MessageReceived));
        }

        [LuisIntent("Order")]
        public async Task Order(IDialogContext context, LuisResult res)
        {
            await Task.Run(async () =>
            {
                bool f = false;
                var r = Validate.ValidateWords(res.Query.ToLower());

                if (r.v) f = true;

                if (f)
                {
                    var cakeOrderForm = new FormDialog<Cakes>(new Cakes(), OrderCake, FormOptions.PromptInStart);
                    context.Call(cakeOrderForm, Callback);
                }
                else
                {
                    await context.PostAsync(Str.cStrDontUnderstand);
                    context.Wait(MessageReceived);
                }
            });
        }
    }
}