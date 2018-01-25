using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Threading.Tasks;

namespace CakeBotSuccinctly.Models
{
    public class Str
    {
        public const string cStrHi = "Hi, I'm CakeBot";
        public const string cStrNameQ = "What is your name?";
        public const string cStrName = "Name";
        public const string cStrGetName = "GetName";

        public const string cStrWhen = "When would you like the cake delivered?";
        public const string cStrProcessingReq = "Thanks for using our service. Delivery has been scheduled for: ";

        public const string cStrDontUnderstand = "I'm sorry I don't understand what you mean.";

        public const string cStrQuantity = "How many?";
        public const string cStrOptions = "Now or Tomorrow";
        public const string cStrDeliverBy = "Deliver by: ";

        public static string[] DeliverTypes = new string[] { "Now", "Tomorrow" };
        public static string[] CakeTypes = new string[] { "Cup Cake", "Triple Layer Cake", "Cream Cake" };

        public static string cStrNoPush = "NO_PUSH";
        public static string cStrTemplateType = "template";
        public static string cStrPayloadTypeGeneric = "generic";
    }

    public class Validate
    {
        public static string DeliverType = string.Empty;

        public static (bool v, string t) TypeExists(string c, string[] types)
        {
            bool v = false;
            string t = string.Empty;

            foreach (string ct in types)
            {
                if (ct.ToLower().Contains(c.ToLower()))
                {
                    v = true;
                    t = ct;

                    break;
                }
            }

            return (v, t);
        }

        public static ValidateResult ValidateType(Cakes state, string value, string[] types)
        {
            ValidateResult result = new ValidateResult { IsValid = false, Value = string.Empty };

            var res = TypeExists(value, types);

            string r = $"{Str.cStrDeliverBy} {res.t}";
            DeliverType = res.t;

            return (res.v) ?
                new ValidateResult { IsValid = true, Value = res.t, Feedback = res.t } : result;
        }
    }

    [Serializable]
    public class Cakes
    {
        [Prompt(Str.cStrQuantity)]
        public string Quantity;

        [Prompt(Str.cStrOptions)]
        public string When;

        public static IForm<Cakes> BuildForm()
        {
            OnCompletionAsyncDelegate<Cakes> processOrder = async (context, state) =>
            {
                context.UserData.SetValue(Str.cStrGetName, true);
                context.UserData.SetValue(Str.cStrName, string.Empty);

                await context.PostAsync($"{Str.cStrProcessingReq} {Validate.DeliverType}");
            };

            return new FormBuilder<Cakes>()
                .Field(nameof(Quantity))

                .Message(Str.cStrWhen)
                .Field(nameof(When),
                    validate: async (state, value) =>
                    {
                        return await Task.Run(() =>
                        {
                            string v = value.ToString();

                            return Validate.ValidateType(state, value.ToString(), Str.DeliverTypes);
                        });
                    })

                .OnCompletion(processOrder)
                .Build();
        }
    }
}