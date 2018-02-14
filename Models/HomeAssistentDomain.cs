using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Models
{
    [Serializable]
    public class HomeAssistentDomain
    {
        public string Domain { get; internal set; }
        public string FriendlyName { get; internal set; }
        public List<HomeAssistentDomainFeature> Features
        {
            get
            {
                return features;
            }
            set
            {
                features = value;
            }
        }

        List<HomeAssistentDomainFeature> features = new List<HomeAssistentDomainFeature>();

        internal void AddFeature(HomeAssistentDomainFeature feature)
        {
            Features.Add(feature);
        }
    }
}