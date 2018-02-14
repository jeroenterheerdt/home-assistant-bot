using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleEchoBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace HomeAssistantBot.Logic
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

        private List<HomeAssistentDomain> _cachedEntitites;

        internal async Task<List<HomeAssistentDomain>> GetEntities()
        {
            //if (_cachedEntitites == null)
            //{ 
            //    var statesAsJson = await GetStates();
            //    JArray deviceStates = JArray.Parse(statesAsJson);

            //    _cachedEntitites = new List<HomeAssistentEntity>();

            //    foreach(var deviceState in deviceStates)
            //    {
            //        var entity = new HomeAssistentEntity();
            //        entity.EntityId = deviceState["entity_id"].Value<string>();
            //        entity.FriendlyName = deviceState["attributes"]?["friendly_name"]?.ToString();

            //        //Debug.WriteLine($"entity_id:{entity_id}\n");

            //        //if (friendlyName != null && friendlyName != "")
            //        //{
            //        _cachedEntitites.Add(entity);
            //        //}
            //    }
            //}
            //return _cachedEntitites;

            if (_cachedEntitites == null)
            {
                var statesAsJson = await GetServices();
                JArray services = JArray.Parse(statesAsJson);

                _cachedEntitites = new List<HomeAssistentDomain>();

                foreach (var service in services.Children<JObject>())
                {
                    var s = service;
                    var entity = new HomeAssistentDomain();
                    entity.Domain = service["domain"].Value<string>();

                    //var sservices = service["services"].Values();
                    var sservices = service["services"];


                    //var x = sservices.Children();
                    foreach (var child in sservices.Value<JObject>())
                    {
                        HomeAssistentDomainFeature feature = new HomeAssistentDomainFeature();
                        feature.Name = child.Key;
                        feature.Description = "";
                        entity.AddFeature(feature);
                    }
                    _cachedEntitites.Add(entity);
                }
            }
            return _cachedEntitites;
        }

        internal async Task<string> GetStates()
        {
            string url = Settings.Instance.BaseApiUrl + "states";
            string resultAsText = await Get(url);
            return resultAsText;
        }

        // Caching
        //Services _cachedServices;

        internal async Task<string> GetServices()
        {
            string url = Settings.Instance.BaseApiUrl + "services";
            string resultAsJson = await Get(url);

            //var dynamicObject= JsonConvert.DeserializeObject(resultAsJson);
            return resultAsJson;
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