using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TransportApiSharpSample
{
    internal class GeoboundingBoxMessage : MessageBase
    {
        public GeoboundingBox Payload { get; set; }

        public GeoboundingBoxMessage(GeoboundingBox message)
        {
            Payload = message;
        }
    }
}