using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net.Http;
using TransportAPISharp;
using TransportApiUnitTests;

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

        [TestMethod]
        public void TestBusTimetable()
        {
            var responsePath = "BusTimetableTest.json";

            var responseString = File.ReadAllText(responsePath);

            var mockHttpHandler = new MockHttpMessageHandler(
                () => new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(responseString)
                });

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHttpHandler);

            var response = client.Timetable("490000077D", new DateTime(2015, 2, 19, 16, 00, 00)).Result;

            Assert.AreEqual(3, response.Departures.All.Count);
        }
    }
}