using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
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
                $"uk/bus/stop/{atcoCode}/{date}/{time}/timetable.json?"
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
                $"uk/bus/stop/{atcoCode}/live.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&group={groupValue}&limit={limit}&nextbuses={nextBusesValue}");

            var jsonString = await task.Content.ReadAsStringAsync();

            return deserializeResponse<BusLiveResponse>(jsonString);
        }

        public async Task<BusRouteTimetableResponse> BusRouteTimetable(string atcoCode, string direction, string lineName, string operatorCode, DateTime dateTime)
        {
            var date = dateTime.ToString("yyyy-mm-dd");
            var time = dateTime.ToString("HH:mm");

            var task = await _httpClient.GetAsync(BaseUrl +
                $"uk/bus/route/{operatorCode}/{lineName}/{direction}/{atcoCode}/{date}/{time}/timetable.json?"
                + $"app_id={_appId}&app_key={_appKey}");

            var jsonString = await task.Content.ReadAsStringAsync();

            return deserializeResponse<BusRouteTimetableResponse>(jsonString);
        }

        public async Task<List<BusOperator>> GetBusOperators()
        {
            var task = await _httpClient.GetAsync(BaseUrl +
                $"uk/bus/operators.json?"
                + $"app_id={_appId}&app_key={_appKey}");

            var jsonString = await task.Content.ReadAsStringAsync();

            return deserializeResponse<List<BusOperator>>(jsonString);
        }

        private T deserializeResponse<T>(string responseString) where T : class
        {
            T returnVal = null;

            // Valid responses are a JSON array, a JSON object with valid data or a JSON object with
            // an 'error' key. Parse the response so we can see if it's an array or an object...
            var parsedResponse = JToken.Parse(responseString);

            if (parsedResponse is JArray)
                // Response is a JSON array, so it's not an error.
                returnVal = JsonConvert.DeserializeObject<T>(responseString);
            else
            {
                // Response is a JSON object. Check to see if it contains an 'error' key...
                var checkForErrors = parsedResponse["error"];
                if (checkForErrors == null)
                    returnVal = JsonConvert.DeserializeObject<T>(responseString);
                else
                    LastError = checkForErrors.Value<string>();
            }

            return returnVal;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}