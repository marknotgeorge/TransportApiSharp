using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportAPISharp;

namespace TransportApiSharpSample.ViewModels
{
    public class DeparturesViewModel
    {
        public ObservableCollection<BusDeparture> Departures { get; set; }
        public string Line { get; set; }
    }
}