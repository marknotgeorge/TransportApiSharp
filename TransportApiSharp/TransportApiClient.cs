using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TransportAPISharp
{
    public class TransportApiClient : IDisposable
    {
        private const string BaseUrl = "http://transportapi.com/v3/";

        private readonly string _appId;
        private readonly string _appKey;

        public TransportApiClient(string appId, string appKey)
        {
            _appId = appId;
            _appKey = appKey;
        }

        public async Task<BusStopsNearResponse> BusStopsNear(double lat, double lon, int page = 1, int rpp = 25)
        {
            BusStopsNearResponse returnVal = null;
            var client = new HttpClient();
            var task = await client.GetAsync("http://transportapi.com/v3/uk/bus/stops/near.json?"
                + $"app_id={_appId}&app_key={_appKey}"
                + $"&lat={lat}&lon={lon}&page={page}&rpp={rpp}");

            var jsonString = await task.Content.ReadAsStringAsync();
            returnVal = JsonConvert.DeserializeObject<BusStopsNearResponse>(jsonString);

            return returnVal;
        }

        public void Dispose()
        {
        }
    }
}