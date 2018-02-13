using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SimpleEchoBot.Logic
{
    [Serializable()]
    public class HomeAssistantService
    {
        private string _baseUrl = "http://localhost:8123/api/";
        public HomeAssistantService()
        {

        }

        internal void TurnOnAllLight()
        {
            
        }

        internal async Task<string> GetStates()
        {
            string url = _baseUrl + "states";
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string resultAsText = await response.Content.ReadAsStringAsync();
            return resultAsText;
        }
    }
}