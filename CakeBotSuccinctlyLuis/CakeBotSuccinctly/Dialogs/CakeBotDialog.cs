using CakeBotSuccinctly.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CakeBotSuccinctly.Dialogs
{
    public class CakeBotDialog
    {
        private const string cStrUser = "User";
        private const string cStrName = "Name";
        private const string cStrTypeOne = "Type one...";

        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(msg => msg.Text)
            .Switch(
            new RegexCase<IDialog<string>>(new Regex("^hi", RegexOptions.IgnoreCase), (context, text) =>
            {
                return Chain.ContinueWith(new WelcomeDialog(), AfterWelcomeContinuation);
            }),
            new DefaultCase<string, IDialog<string>>((context, text) =>
            {
                return Chain.ContinueWith(FormDialog.FromForm(Cakes.BuildForm, FormOptions.PromptInStart), AfterFormFlowContinuation);
            }))
            .Unwrap()
            .PostToUser();

        private async static Task<IDialog<string>> AfterWelcomeContinuation(IBotContext context, IAwaitable<object> item)
        {
            var tk = await item;
            string name = cStrUser;
            context.UserData.TryGetValue(cStrName, out name);
            return Chain.Return($"{cStrTypeOne} {name}");
        }

        private async static Task<IDialog<string>> AfterFormFlowContinuation(IBotContext context, IAwaitable<object> item)
        {
            var tk = await item;
            return Chain.Return("Thanks, you're awesome");
        }
    }
}