using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportApiSharpSample.Models
{
    public class BusStopParameter
    {
        public string StopName { get; set; }
        public string AtcoCode { get; set; }

        public BusStopParameter(string stopName, string atcoCode)
        {
            StopName = stopName;
            AtcoCode = atcoCode;
        }
    }
}