using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using SimpleEchoBot.Logic;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;
        HomeAssistantService _homeAssistant;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            if (_homeAssistant == null)
            {
                _homeAssistant = new HomeAssistantService();
            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            string text = message.Text.ToLower();

            if (text == "get state")
            {
                string result = await _homeAssistant.GetStates();
                await context.PostAsync($"states: {result}");
                context.Wait(MessageReceivedAsync);
            }
            else if (text == "turn on")
            {
                _homeAssistant.TurnOnAllLight();
            }
            else if (text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}