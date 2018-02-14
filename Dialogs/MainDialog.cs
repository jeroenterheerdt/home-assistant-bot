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

namespace HomeAssistantBot
{
    [Serializable]
    [LuisModel(Settings.LuisAppId, Settings.LuisAPIKey)]
    public class MainDialog : LuisDialog<object>
    {
        protected int count = 1;
        HomeAssistantService _homeAssistant;

        string LuisModelUrl = "https://" + Settings.Instance.LuisAPIHostName + "/luis/v1/application?id=" + Settings.LuisAppId + "&subscription-key=" + Settings.LuisAPIKey;
        enum EntityTypes { Room, Device};

        public async Task StartAsync(IDialogContext context)
        {
            
            //context.Wait(MessageReceivedAsync);

            if (_homeAssistant == null)
            {
                _homeAssistant = new HomeAssistantService();
            }
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry, I did not understand that. Ask for help to get help.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Hi")]
        public async Task Hi(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hi, I am {Settings.Instance.BotName}. I can help you to manage your smart home.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Need help? Here is an idea: you can say things like 'turn on living room lights'.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Turn On")]
        public async Task TurnOn(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if(result.Entities.Count > 0)
            {
                string device = GetFirstEntityForType(result, EntityTypes.Device);
                await context.PostAsync($"You asked to TURN ON the {device}.");
            }
            else
            {
                await context.PostAsync($"Not sure what to turn on. Please make sure to specify an entity.");
            }
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Turn Off")]
        public async Task TurnOff(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (result.Entities.Count > 0)
            {
                string device = GetFirstEntityForType(result, EntityTypes.Device);
                await context.PostAsync($"You asked to TURN OFF the {device}.");
            }
            else
            {
                await context.PostAsync($"Not sure what to turn off. Please make sure to specify an entity.");
            }
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Get Entities")]
        public async Task GetEntities(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            await context.PostAsync("You reached the GET ENTITIES intent");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Get State")]
        public async Task GetState(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (result.Entities.Count > 0)
            {
                string room  = GetFirstEntityForType(result, EntityTypes.Room);
                string device = GetFirstEntityForType(result, EntityTypes.Device);

                await context.PostAsync($"You asked to GET STATE of the {device} in {room}.");
            }
            else
            {
                await context.PostAsync($"Not sure what entity you want to get the state for. Please make sure to specify an entity.");
            }
            context.Wait(this.MessageReceived);
        }

        

        [LuisIntent("Set State")]
        public async Task SetState(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            if (result.Entities.Count > 1)
            {
                string device = GetFirstEntityForType(result, EntityTypes.Device);
                string room = GetFirstEntityForType(result, EntityTypes.Room);
                
                await context.PostAsync($"You asked to SET STATE of the {device} in {room}.");
            }
            else
            {
                await context.PostAsync($"Not sure what entity you want to set the state for and to what value. Please make sure to specify an entity and value.");
            }
            context.Wait(this.MessageReceived);
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

        /*public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            string text = message.Text.ToLower();

            string responseText = "";

            if (text == "get state")
            {
                string result = await _homeAssistant.GetStates();

                responseText = $"states: {result}";
                
            }
            else if (text == "get services")
            {
                var services = await _homeAssistant.GetServices();
                responseText = "Get services was successful";   
            }
            else if (text == "get entities")
            {
                var entities = await _homeAssistant.GetEntities();
                responseText = "Get services was successful\n\n";
                foreach(var entity in entities)
                {
                    responseText += $"- {entity.Domain}\n\n";
                    foreach(var feature in entity.Features)
                    {
                        responseText += $" \t- {feature.Name}\n\n";
                    }
                }
            }
            else if (text == "turn on")
            {
                _homeAssistant.TurnOnAllLight();
                responseText = "Hopefully all lights are on now";
            }
            else if (text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
                return;
            }
            else
            {
                responseText = $"{this.count++}: You said {message.Text}";
            }

            await context.PostAsync(responseText);
            context.Wait(MessageReceivedAsync);
        }
        */
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
            //context.Wait(MessageReceivedAsync);
        }

    }
}