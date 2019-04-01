using Newtonsoft.Json.Linq;
using RoadStatus.Core.Interfaces;
using RoadStatus.Core.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoadStatus.Clients
{
    public class RoadStatusRestClient : IRoadStatusClient
    {
        private IConfigurationManager configurationManager;
        private HttpClient httpClient;

        public RoadStatusRestClient(IConfigurationManager configurationManager)
            : this(configurationManager, new HttpClient()) { }

        public RoadStatusRestClient(IConfigurationManager configurationManager, HttpClient httpClient)
        {
            this.configurationManager = configurationManager;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Gets the status of the road by contacting the TfL API
        /// </summary>
        /// <param name="roadId">The ID of the road</param>
        /// <returns>The status of the road</returns>
        public async Task<RoadStatusResponse> GetRoadStatusAsync(string roadId)
        {
            if (string.IsNullOrWhiteSpace(roadId))
            {
                throw new ArgumentException($"Argument '{nameof(roadId)}' is invalid");
            }

            string appId = configurationManager.GetConfigValue("AppId");
            string appKey = configurationManager.GetConfigValue("AppKey");
            string apiUri = configurationManager.GetConfigValue("ApiUri");

            HttpResponseMessage httpResponse = await this.httpClient
                                                .GetAsync($"{apiUri}{roadId}?app_id={appId}&app_key={appKey}");

            RoadStatusResponse roadStatus = new RoadStatusResponse();
            switch (httpResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    {
                        string responseJson = await httpResponse.Content.ReadAsStringAsync();
                        // Find the first road status matching the passed road ID
                        JObject parsedJson = JArray.Parse(responseJson)
                                                .Children<JObject>()
                                                .FirstOrDefault(o => o["id"].ToString().ToLower() == roadId.ToLower());

                        roadStatus.IsRoadFound = true;
                        roadStatus.DisplayName = parsedJson["displayName"].ToString();
                        roadStatus.StatusSeverity = parsedJson["statusSeverity"].ToString();
                        roadStatus.StatusSeverityDescription = parsedJson["statusSeverityDescription"].ToString();
                        break;
                    }
                case System.Net.HttpStatusCode.NotFound:
                    {
                        roadStatus.DisplayName = roadId;
                        break;
                    }
                default:
                    {
                        throw new ApplicationException("An error occured while contacting the TfL REST API.");
                    }
            }
            return roadStatus;

        }
    }
}
