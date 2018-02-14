using Newtonsoft.Json;
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
            string resultAsText = await Get(url);
            return resultAsText;
        }

        // Caching
        //Services _cachedServices;

        internal async Task<dynamic> GetServices()
        {
            string url = _baseUrl + "services";
            string resultAsJson = await Get(url);

            var dynamicObject= JsonConvert.DeserializeObject(resultAsJson);
            return dynamicObject;
        }

        private async Task<string> Get(string url)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            string resultAsText = await response.Content.ReadAsStringAsync();
            return resultAsText;
        }
    }
}