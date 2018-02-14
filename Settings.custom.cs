using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot
{
    public partial class Settings
    {
        public const string LuisAppId = "23dda9ed-2af5-4100-8e3b-18f9f730a0a4";
        public const string LuisAPIKey = "d46e0c5567c44936a97b32d9dc794c18";

        public Settings()
        {
            this.BotName = "Cortana, the Home Assistant";
            this.BaseApiUrl = "http://localhost:8123/api/";
            this.LuisAPIHostName = "westus.api.cognitive.microsoft.com";
        }

    }
}