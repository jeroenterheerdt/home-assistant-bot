/**
 * Create a class called Settings.custom.cs and paste in this code:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAssistantBot
{
    public partial class Settings
    {
        public const string LuisAppId = "YOUR LUIS APP ID";
        public const string LuisAPIKey = "YOUR LUIS API KEY";

        public Settings()
        {
            this.BotName = "YOUR BOT NAME";
            this.BaseApiUrl = "YOUR BASE API URL";
            this.LuisAPIHostName = "YOUR LUIS API HOST NAME";
        }
    }
}*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeAssistantBot
{
    public partial class Settings
    {
        // Sample Settings
        public string BotName { get; set; }
        public string BaseApiUrl { get; set; }
        public string LuisAPIHostName { get; set; }
        public string LuisSubscriptionKey { get; set; }
        public string LuisAppId { get; set; }
        public string HomeAssistantPassword { get; set; }

        private static Settings _settings;
        public static Settings Instance
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new Settings();
                }
                return _settings;
            }
        }
    }
}