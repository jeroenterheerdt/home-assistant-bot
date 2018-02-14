using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot
{
    public partial class Settings
    {
        // Sample Settings
        public string BotName { get; set; }

        public string BaseApiUrl { get; set; }

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