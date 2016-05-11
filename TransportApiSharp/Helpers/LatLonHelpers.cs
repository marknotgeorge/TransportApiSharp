using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportApiSharp.Helpers
{
    public static class LatLonHelpers
    {
        public static string LocationString(double latitude, double longitude)
        {
            return $"lonlat:{longitude},{latitude}";
        }
    }
}