using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using EntireData.DTO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EntireData
{
    public class Provider:IProvider
    {
        private const string BASE_ADDRESS = "http://environment.data.gov.uk/flood-monitoring/id/stations";
        private HttpClient _httpClient;

        public Provider()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }       



        public List<Station> GetStations(string riverName)
        {
            if (string.IsNullOrEmpty(riverName))
                return null;

            _httpClient.BaseAddress =new Uri(BASE_ADDRESS + "?riverName=" + riverName);
            var response =  _httpClient.GetAsync("").GetAwaiter().GetResult();
            JObject jsonResult = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return jsonResult["items"].ToObject<Station[]>().ToList();
            
        }

        public Task<StationResults> GetMeasureAsync(string station)
        {
            if (string.IsNullOrEmpty(station))
                return null;
        

            return Task<StationResults>.Factory.StartNew(() =>
            {
                var client = new HttpClient { BaseAddress = new Uri(BASE_ADDRESS + "/" + station 
                                                                                 + "/readings?_sorted&_limit=9999999999999&since=" 
                                                                                 + DateTime.UtcNow.AddDays(-7).ToString("yyyy-MM-ddThh:mm:ss")
                                                                                 ) };
                var response = client.GetAsync("").GetAwaiter().GetResult();
                JObject jsonResult = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                return new StationResults
                {
                    Measures = jsonResult["items"].ToObject<Measure[]>().ToList<Measure>(),
                    RLOIid = station
                };
            });

       

        

        }
    }
}
