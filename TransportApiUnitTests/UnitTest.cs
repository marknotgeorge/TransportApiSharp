using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
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
            var client = new TransportApiClient(ApiCredentials.apiKey, ApiCredentials.apiSecret);

            var response = client.BusStopsNear(51.4728, -0.4876).Result;

            Assert.AreEqual(1918, response.total);
        }
    }
}