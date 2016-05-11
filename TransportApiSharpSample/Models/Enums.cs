using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportApiSharpSample.Models
{
    [Flags]
    public enum TransportMode
    {
        Bus = 1,
        Train = 2,
        Tube = 4
    }
}