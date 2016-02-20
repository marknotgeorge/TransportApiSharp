using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TransportAPISharp;

namespace TransportAPISharpUnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestBusStopsNear()
        {
            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey);

            var response = client.BusStopsNear(51.4728, -0.4876).Result;

            Assert.AreEqual(1918, response.total);
        }
    }
}