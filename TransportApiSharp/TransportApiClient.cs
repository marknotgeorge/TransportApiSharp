using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TransportAPISharp
{
    public class TransportApiClient : IDisposable
    {
        private const string BaseUrl = "http://transportapi.com/v3/";

        private readonly string _appId;
        private readonly string _appKey;

        private HttpClient _httpClient;

        public TransportApiClient(string appId, string appKey, HttpMessageHandler handler = null)
        {
            _appId = appId;
            _appKey = appKey;
            if (handler != null)
                _httpClient = new HttpClient(handler);
            else
                _httpClient = new HttpClient();
        }

        public async Task<BusStopsNearResponse> BusStopsNear(double lat, double lon, int page = 1, int rpp = 25)
        {
            BusStopsNearResponse returnVal = null;
            var task = await _httpClient.GetAsync("http://transportapi.com/v3/uk/bus/stops/near.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&lat={lat}&lon={lon}&page={page}&rpp={rpp}");

            var jsonString = await task.Content.ReadAsStringAsync();
            returnVal = JsonConvert.DeserializeObject<BusStopsNearResponse>(jsonString);

            return returnVal;
        }

        public async Task<BusTimetableResponse> Timetable(string atcoCode, DateTime dateTime, int limit = 3)
        {
            BusTimetableResponse returnVal = null;
            var date = dateTime.ToString("yyyy-MM-dd");
            var time = dateTime.ToString("HH:mm");

            var task = await _httpClient.GetAsync(BaseUrl +
                $"/uk/bus/stop/{atcoCode}/{date}/{time}/timetable.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&group=no&limit={limit}");

            var jsonString = await task.Content.ReadAsStringAsync();

            returnVal = JsonConvert.DeserializeObject<BusTimetableResponse>(jsonString);

            return returnVal;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}