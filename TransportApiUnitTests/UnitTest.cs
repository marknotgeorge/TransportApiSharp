using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using System;
using System.IO;
using TransportApiSharp.Helpers;
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
        public void TestBusStopsInBoundingBox()
        {
            var mockHandler = getHandler("BusStopsBoundingBoxResponse.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.BusStopsInBoundingBox(51.5231, -0.10475, 51.51988, -0.10958).Result;

            Assert.AreEqual(7, response.total);
        }

        [TestMethod]
        public void TestBusTimetableGrouped()
        {
            var mockHandler = getHandler("BusTimeTableGroupedResponse.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.BusTimetable("490000077D", new DateTime(2015, 2, 19, 16, 00, 00)).Result;

            Assert.AreEqual(4, response.Departures.Count);
        }

        [TestMethod]
        public void TestBusTimetable()
        {
            var mockHandler = getHandler("BusTimetableTest.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.BusTimetable("490000077D", new DateTime(2015, 2, 19, 16, 00, 00), false).Result;

            Assert.AreEqual(3, response.Departures["all"].Count);
        }

        [TestMethod]
        public void TestBusLive()
        {
            var mockHandler = getHandler("BusLiveResponse.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.BusLive("490000077D", new DateTime(2015, 2, 19, 16, 00, 00)).Result;

            Assert.AreEqual(4, response.Departures.Count);
        }

        [TestMethod]
        public void TestBusOperators()
        {
            var mockHandler = getHandler("BusOperatorsResponse.json");

            var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey, mockHandler);

            var response = client.GetBusOperators().Result;

            Assert.AreEqual(3, response.Count);
        }

        [TestMethod]
        public void TestBusOperatorsError()
        {
            var client = new TransportApiClient(string.Empty, string.Empty);

            var errorString = "Authorisation failed for app_key  and app_id  with error 'application with id=\"\" was not found' (code 'application_not_found'). See http://transportapi.com for plans and sign-up.";

            var response = client.GetBusOperators().Result;

            Assert.AreEqual(null, response);
            Assert.AreEqual(errorString, client.LastError);
        }

        [TestMethod]
        public void TestLocationString()
        {
            var returnString = LatLonHelpers.LocationString(51.4728, -0.4876);

            Assert.AreEqual("lonlat:-0.4876,51.4728", returnString);
        }
    }
}