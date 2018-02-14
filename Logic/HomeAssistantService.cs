using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleEchoBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace SimpleEchoBot.Logic
{
    [Serializable()]
    public class HomeAssistantService
    {
        
        public HomeAssistantService()
        {

        }

        private List<HomeAssistentEntity> _cachedEntitites;

        internal async Task<List<HomeAssistentEntity>> GetEntities()
        {
            if (_cachedEntitites == null)
            {
                var json = await GetStates();
                JArray states = JArray.Parse(json);

                //_cachedDomains = new List<HomeAssistentDomain>();
                _cachedEntitites = new List<HomeAssistentEntity>();

                foreach (var state in states.Children<JObject>())
                {
                    var e = new HomeAssistentEntity();

                    e.EntityId = state["entity_id"].Value<string>();
                    e.FriendlyName = state["attributes"]?["friendly_name"]?.Value<string>();
                    _cachedEntitites.Add(e);

                    //    var s = service;
                    //    var entity = new HomeAssistentDomain();
                    //    entity.Domain = service["domain"].Value<string>();

                    //    //var sservices = service["services"].Values();
                    //    var sservices = service["services"];


                    //    //var x = sservices.Children();
                    //    foreach (var child in sservices.Value<JObject>())
                    //    {
                    //        HomeAssistentDomainFeature feature = new HomeAssistentDomainFeature();
                    //        feature.Name = child.Key;
                    //        feature.Description = "";
                    //        entity.AddFeature(feature);
                    //    }
                    //    _cachedDomains.Add(entity);
                }
            }
            return _cachedEntitites;
        }

        private List<HomeAssistentDomain> _cachedDomains;

        internal async Task<List<HomeAssistentDomain>> GetDomains()
        {
            if (_cachedDomains == null)
            {
                var statesAsJson = await GetServices();
                JArray services = JArray.Parse(statesAsJson);

                _cachedDomains = new List<HomeAssistentDomain>();

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
                    _cachedDomains.Add(entity);
                }
            }
            return _cachedDomains;
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


        async internal void ToggleAllLights()
        {
            var entities = await GetEntities();
            foreach (var entity in entities)
            {
                if (entity.EntityId.StartsWith("light."))
                {
                    await ToggleLight(entity.EntityId);
                }
            }
        }

        async internal void AllLightsOn()
        {
            var entities = await GetEntities();
            foreach (var entity in entities)
            {
                if (entity.EntityId.StartsWith("light."))
                {
                    await TurnLightOn(entity.EntityId);
                }
            }
        }

        async internal void AllLightsOff()
        {
            var entities = await GetEntities();
            foreach (var entity in entities)
            {
                if (entity.EntityId.StartsWith("light."))
                {
                    await TurnLightOff(entity.EntityId);
                }
            }
        }

        internal async Task<bool> ToggleLight(string entityId)
        {
            string url = Settings.Instance.BaseApiUrl + "services/light/toggle";

            string json = "{\"entity_id\": \"" + entityId + "\" }";

            var result = await SetState(url, json);
            return result;
        }

        internal async Task<bool> TurnLightOn(string entityId)
        {
            string url = Settings.Instance.BaseApiUrl + "services/light/turn_on";

            string json = "{\"entity_id\": \"" + entityId + "\" }";

            var result = await SetState(url, json);
            return result;
        }

        internal async Task<bool> TurnLightOff(string entityId)
        {
            string url = Settings.Instance.BaseApiUrl + "services/light/turn_off";

            string json = "{\"entity_id\": \"" + entityId + "\" }";

            var result = await SetState(url, json);
            return result;
        }

        internal async Task<bool> SetState(string url, string json)
        {
            try
            {
                //http://localhost:8123/api/states/light.bed_light
                //string url = Settings.Instance.BaseApiUrl + "states/" + entityId;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json; charset=utf-8";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        //pass.Text = result.ToString();
                    }
                }

                //HttpClient httpClient = new HttpClient();
                //string json = "{'state':'" + state + "'}";

                //var response = await httpClient.PostAsJsonAsync(url, json);
                //string resultAsText = await response.Content.ReadAsStringAsync();

                return true;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return false;
            }
        }
    }
}