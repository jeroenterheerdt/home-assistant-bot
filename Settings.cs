using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HomeAssistantBot
{
    public class Settings
    {
        // Sample Settings
        public string BotName
        {
            get
            {
                return WebConfigurationManager.AppSettings["BotName"];
            }
        }
        public string BaseApiUrl
        {
            get
            {
                return WebConfigurationManager.AppSettings["BaseApiUrl"];
            }
        }
        public string LuisAPIHostName
        {
            get
            {
                return WebConfigurationManager.AppSettings["LuisAPIHostName"];
            }
        }
        public string LuisSubscriptionKey {
            get
            {
                return WebConfigurationManager.AppSettings["LuisSubscriptionKey"];
            }
        }
        public string LuisAppId
        {
            get
            {
                return WebConfigurationManager.AppSettings["LuisAppId"];
            }
        }
        public string HomeAssistantPassword
        {
            get
            {
                return WebConfigurationManager.AppSettings["HomeAssistantPassword"]; 
            }
        }

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

        public Settings()
        {
        }
    }
}