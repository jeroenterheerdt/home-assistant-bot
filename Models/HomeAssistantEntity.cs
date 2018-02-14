using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Models
{
    [Serializable]
    public class HomeAssistentEntity
    {
        public string EntityId { get; set; }
        public string FriendlyName { get; set; }
    }
}