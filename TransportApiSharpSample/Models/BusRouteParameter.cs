using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportApiSharpSample.Models
{
    public class BusRouteParameter
    {
        public string AtcoCode { get; set; }
        public string Direction { get; set; }
        public string LineName { get; set; }
        public string OperatorCode { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}