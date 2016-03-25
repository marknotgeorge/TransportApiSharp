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
            BusStopsNearResponse returnVal = null;
            var task = await _httpClient.GetAsync("http://transportapi.com/v3/uk/bus/stops/near.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&lat={lat}&lon={lon}&page={page}&rpp={stopsPerPage}");

            var jsonString = await task.Content.ReadAsStringAsync();
            returnVal = JsonConvert.DeserializeObject<BusStopsNearResponse>(jsonString);

            return returnVal;
        }

        public async Task<BusTimetableResponse> Timetable(string atcoCode, DateTime dateTime, bool group = true, int limit = 3)
        {
            BusTimetableResponse returnVal = null;
            var date = dateTime.ToString("yyyy-MM-dd");
            var time = dateTime.ToString("HH:mm");

            var groupValue = (group) ? "route" : "no";

            var task = await _httpClient.GetAsync(BaseUrl +
                $"/uk/bus/stop/{atcoCode}/{date}/{time}/timetable.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&group={groupValue}&limit={limit}");

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