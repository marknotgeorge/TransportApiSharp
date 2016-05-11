using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TransportAPISharp
{
    /// <summary>
    /// A .Net client for the <a href="http://www.transportapi.com">TransportAPI</a> UK public
    /// transport API.
    /// </summary>
    public class TransportApiClient : IDisposable
    {
        private const string BaseUrl = "http://transportapi.com/v3/";

        private readonly string _appId;
        private readonly string _appKey;

        private HttpClient _httpClient;

        public string LastError { get; set; }

        /// <summary>
        /// Initialises a new <c>TransportApiClient</c> class.
        /// </summary>
        /// <param name="appId">The application's Id. Get this from the TransportAPI site.</param>
        /// <param name="appKey">The application's key. Get this from the TransportAPI site.</param>
        /// <param name="handler">
        /// Used to inject an alternative <c>System.Net.Http.HttpMessageHandler</c>. Used for unit testing.
        /// </param>
        public TransportApiClient(string appId, string appKey, HttpMessageHandler handler = null)
        {
            _appId = appId;
            _appKey = appKey;
            if (handler != null)
                _httpClient = new HttpClient(handler);
            else
                _httpClient = new HttpClient();
        }

        /// <summary>
        /// Returns bus stops near a given geographic position.
        /// </summary>
        /// <param name="lat">The latitude of the position in question.</param>
        /// <param name="lon">The longitude of the position in question.</param>
        /// <param name="page">The page number of the result set.</param>
        /// <param name="stopsPerPage"></param>
        /// <returns>A <c>BusStopsNearResponse</c> class</returns>
        public async Task<BusStopsNearResponse> BusStopsNear(double lat, double lon, int page = 1, int stopsPerPage = 25)
        {
            var task = await _httpClient.GetAsync("http://transportapi.com/v3/uk/bus/stops/near.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&lat={lat}&lon={lon}&page={page}&rpp={stopsPerPage}");

            var jsonString = await task.Content.ReadAsStringAsync();
            return deserializeResponse<BusStopsNearResponse>(jsonString);
        }

        public async Task<BusStopsNearResponse> BusStopsInBoundingBox(double north, double south, double east, double west, int page = 1, int stopsPerPage = 25)
        {
            var task = await _httpClient.GetAsync(BaseUrl +
                "uk/bus/stops/bbox.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&maxlat={north}&maxlon={east}"
                + $"&minlat={south}&minlon={west}"
                + $"&page={page}&rpp={stopsPerPage}");

            var jsonString = await task.Content.ReadAsStringAsync();
            return deserializeResponse<BusStopsNearResponse>(jsonString);
        }

        public async Task<BusTimetableResponse> BusTimetable(string atcoCode, DateTime dateTime, bool group = true, int limit = 3)
        {
            var date = dateTime.ToString("yyyy-MM-dd");
            var time = dateTime.ToString("HH:mm");

            var groupValue = (group) ? "route" : "no";

            var task = await _httpClient.GetAsync(BaseUrl +
                $"/uk/bus/stop/{atcoCode}/{date}/{time}/timetable.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&group={groupValue}&limit={limit}");

            var jsonString = await task.Content.ReadAsStringAsync();

            return deserializeResponse<BusTimetableResponse>(jsonString);
        }

        public async Task<BusLiveResponse> BusLive(string atcoCode, bool group = true, int limit = 3, bool nextBuses = false)
        {
            var groupValue = (group) ? "route" : "no";
            var nextBusesValue = (nextBuses) ? "yes" : "no";

            var task = await _httpClient.GetAsync(BaseUrl +
                $"/uk/bus/stop/{atcoCode}/live.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&group={groupValue}&limit={limit}&nextbuses={nextBusesValue}");

            var jsonString = await task.Content.ReadAsStringAsync();

            return deserializeResponse<BusLiveResponse>(jsonString);
        }

        public async Task<List<BusOperator>> GetBusOperators()
        {
            var task = await _httpClient.GetAsync(BaseUrl +
                $"/uk/bus/operators.json?"
                + $"app_id={_appId}&app_key={_appKey}");

            var jsonString = await task.Content.ReadAsStringAsync();

            return deserializeResponse<List<BusOperator>>(jsonString);
        }

        private T deserializeResponse<T>(string responseString)
        {
            try
            {
                var returnVal = JsonConvert.DeserializeObject<T>(responseString);
                return returnVal;
            }
            catch (Exception)
            {
                var jObject = JObject.Parse(responseString);
                LastError = (string)jObject["error"];
                return default(T);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}