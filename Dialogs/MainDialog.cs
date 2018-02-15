using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net.Http;
using HomeAssistantBot.Logic;
using Microsoft.Bot.Builder.Luis;
using System.Linq;
using System.Collections.Generic;
using Autofac;
using System.Configuration;
using Microsoft.Bot.Builder.Internals.Fibers;
using System.Web.Configuration;
using System.Threading;

namespace HomeAssistantBot
{
    [Serializable]
    public class MainDialog : IDialog<object>
    {
        protected int count = 1;
        //string LuisModelUrl = "https://" + Settings.Instance.LuisAPIHostName + "/luis/v1/application?id=" + Settings.LuisAppId + "&subscription-key=" + Settings.LuisAPIKey;
        enum EntityTypes { Room, Device};
        LuisService luis;

        public async Task StartAsync(IDialogContext context)
        {
            luis = new LuisService(
                new LuisModelAttribute(
                    Settings.Instance.LuisAppId,
                    Settings.Instance.LuisSubscriptionKey,
                    LuisApiVersion.V2,
                    domain: Settings.Instance.LuisAPIHostName
                    ));

            context.Wait(MessageReceivedAsync);
        }

        private HomeAssistantService _homeAssistant;
        private HomeAssistantService HomeAssistant
        {
            get
            {
                if (_homeAssistant == null)
                {
                    _homeAssistant = new HomeAssistantService();
                }
                return _homeAssistant;
            }
        }

        private static IMessageActivity MakeMessage(IDialogContext context, string speak, string text)
        {
            var act = context.MakeMessage();
            act.Speak = speak;
            act.Text = text;
            //act.InputHint = InputHints.AcceptingInput;
            return act;
        }

        private string GetFirstEntityForType(LuisResult result, EntityTypes v)
        {
            var r = GetEntitiesForType(result, v);
            if (r.Count() == 0)
            {
                return null;
            }
            else
            {
                return r.FirstOrDefault().Entity;
            }
        }

        private static System.Collections.Generic.IEnumerable<EntityRecommendation> GetEntitiesForType(LuisResult result, EntityTypes entityType)
        {
            return from ent in result.Entities
                   where ent.Type == entityType.ToString()
                   select ent;

        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var luisResult = await luis.QueryAsync(message.Text, CancellationToken.None);

            switch (luisResult.TopScoringIntent.Intent)
            {
                case "Turn On":
                    await HomeAssistant.AllLightsOn();
                    string msg = "I turned on all the lights.";
                    await context.PostAsync(MakeMessage(context, msg, msg));
                    break;
                case "Turn Off":
                    await HomeAssistant.AllLightsOff();
                    string msg2 = "I turned off all the lights.";
                    await context.PostAsync(MakeMessage(context, msg2, msg2));
                    break;
                case "Hi":
                    string msg3 = $"Hi, I am { Settings.Instance.BotName}. I can help you to manage your smart home.";
                    await context.PostAsync(MakeMessage(context, msg3, msg3));
                    break;
                case "Get Entities":
                    var entities = await HomeAssistant.GetDomains();
                    string responseText = "Get services was successful\n\n";
                    foreach (var entity in entities)
                    {
                        responseText += $"- {entity.Domain}\n\n";
                        foreach (var feature in entity.Features)
                        {
                            responseText += $" \t- {feature.Name}\n\n";
                        }
                    }
                    await context.PostAsync(responseText);
                    break;
                case "Get State":
                    if (luisResult.Entities.Count > 0)
                    {
                        string room = GetFirstEntityForType(luisResult, EntityTypes.Room);
                        string device = GetFirstEntityForType(luisResult, EntityTypes.Device);

                        await context.PostAsync($"You asked to GET STATE of the {device} in {room}.");
                    }
                    else
                    {
                        await context.PostAsync($"Not sure what entity you want to get the state for. Please make sure to specify an entity.");
                    }
                    break;
                case "Set State":
                    if (luisResult.Entities.Count > 1)
                    {
                        string device = GetFirstEntityForType(luisResult, EntityTypes.Device);
                        string room = GetFirstEntityForType(luisResult, EntityTypes.Room);

                        await context.PostAsync($"You asked to SET STATE of the {device} in {room}.");
                    }
                    else
                    {
                        await context.PostAsync($"Not sure what entity you want to set the state for and to what value. Please make sure to specify an entity and value.");
                    }
                    break;
                default:
                    await context.PostAsync(MakeMessage(context, "I'm sorry, I did not get that.", "I'm sorry, I did not understand that. Ask for help to get help."));
                    break;
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}