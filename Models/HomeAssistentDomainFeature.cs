using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Models
{
    [Serializable]
    public class HomeAssistentDomainFeature
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }
}