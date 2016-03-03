using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportApiSharpSample.Models;

namespace TransportApiSharpSample.Messages
{
    internal class BusStopMessage : MessageBase
    {
        public BusStop Payload { get; set; }

        public BusStopMessage(BusStop payload)
        {
            Payload = payload;
        }
    }
}