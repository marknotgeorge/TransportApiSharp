using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportApiSharpSample.Models;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using WpWinNl.Maps;

namespace TransportApiSharpSample.MapDrawers
{
    internal class BusStopDrawer : MapIconDrawer
    {
        public override MapElement CreateShape(object viewModel, BasicGeoposition pos)
        {
            var shape = (MapIcon)base.CreateShape(viewModel, pos);
            var busStop = (BusStop)viewModel;
            shape.Title = busStop.Title;
            shape.Image = busStop.Image;
            return shape;
        }
    }
}