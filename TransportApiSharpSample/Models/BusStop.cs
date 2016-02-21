using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;

namespace TransportApiSharpSample.Models
{
    public class BusStop
    {
        public BasicGeoposition Point { get; set; }

        public string Title { get; set; }

        public string AtcoCode { get; set; }

        public IRandomAccessStreamReference Image { get; set; }

        public BusStop(double latitude, double longitude, string title, string atcoCode = "", IRandomAccessStreamReference image = null)
        {
            var point = new BasicGeoposition()
            {
                Latitude = latitude,
                Longitude = longitude
            };

            Point = point;
            Title = title;
            AtcoCode = atcoCode;
            Image = image;
        }
    }
}