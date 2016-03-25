using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.IO;
using TransportAPISharp;

namespace TransportAPISharpUnitTests
{
    [TestClass]
    public class UnitTests
    {
        private MockHttpMessageHandler getHandler(string responseFile)
        {
            var responseString = File.ReadAllText(responseFile);

            var mockHttpHandler = new MockHttpMessageHandler();

            mockHttpHandler.When("http://*").Respond("application/json", responseString);

            return mockHttpHandler;
        }

        [TestMethod]
        public void TestBusStopsNear()
        {
            var mockHandler = getHandler("BusStopNearResponse.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.BusStopsNear(51.4728, -0.4876).Result;

            Assert.AreEqual(1937, response.total);
        }

        [TestMethod]
        public void TestBusTimetableGrouped()
        {
            var mockHandler = getHandler("BusTimeTableGroupedResponse.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.Timetable("490000077D", new DateTime(2015, 2, 19, 16, 00, 00)).Result;

            Assert.AreEqual(4, response.Departures.Count);
        }

        [TestMethod]
        public void TestBusTimetable()
        {
            var mockHandler = getHandler("BusTimetableTest.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.Timetable("490000077D", new DateTime(2015, 2, 19, 16, 00, 00), false).Result;

            Assert.AreEqual(3, response.Departures["all"].Count);
        }
    }
}