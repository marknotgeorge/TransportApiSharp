using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportAPISharp
{
    public class BusStopsNearResponse
    {
        public float minlon { get; set; }
        public float minlat { get; set; }
        public float maxlon { get; set; }
        public float maxlat { get; set; }
        public float searchlon { get; set; }
        public float searchlat { get; set; }
        public int page { get; set; }
        public int rpp { get; set; }
        public int total { get; set; }
        public DateTime request_time { get; set; }
        public Stop[] stops { get; set; }
    }

    public class Stop
    {
        public string atcocode { get; set; }
        public string smscode { get; set; }
        public string name { get; set; }
        public string mode { get; set; }
        public string bearing { get; set; }
        public string locality { get; set; }
        public string indicator { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int distance { get; set; }
    }
}