using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleEchoBot.Logic
{
    [Serializable()]
    public class HomeAssistantService
    {
        
        public HomeAssistantService()
        {

        }

        internal void TurnOnAllLight()
        {
            
        }

        internal async Task<List<string>> GetEntities()
        {
            var statesAsJson = await GetStates();

            //var arrayOfDeviceStates = JsonConvert.DeserializeObject(statesAsJson);
            JArray deviceStates = JArray.Parse(statesAsJson);

            foreach(var deviceState in deviceStates)
            {
                //Debug.WriteLine(deviceState.ToString());
                //string entity_id = deviceState["entity_id"].Value<string>();
                //dynamic attributes = deviceState["attributes"];
                //var friendly_name = attributes.friendly_name;
                ////?.Values()["friendly_name"]?.Value<string>();
                //Debug.WriteLine($"entity_id:{entity_id}\n");

            }

            return null;

        }

        internal async Task<string> GetStates()
        {
            string url = Settings.Instance.BaseApiUrl + "states";
            string resultAsText = await Get(url);
            return resultAsText;
        }

        // Caching
        //Services _cachedServices;

        internal async Task<dynamic> GetServices()
        {
            string url = Settings.Instance.BaseApiUrl + "services";
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