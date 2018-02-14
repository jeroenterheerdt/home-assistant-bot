using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot
{
    public partial class Settings
    {
        public Settings()
        {
            this.BotName = "Cortana, the Home Assistant";
            this.BaseApiUrl = "http://localhost:8123/api/";
        }

        
    }
}