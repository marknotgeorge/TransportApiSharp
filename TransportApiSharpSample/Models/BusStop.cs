using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TransportApiSharpSample.Messages;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using WpWinNl.Maps;

namespace TransportApiSharpSample.Models
{
    public class BusStop
    {
        public BasicGeoposition Point { get; set; }

        public string Title { get; set; }

        public string AtcoCode { get; set; }

        public IRandomAccessStreamReference Image { get; set; }

        public ICommand SelectCommand => new RelayCommand<MapSelectionParameters>(Select);

        public void Select(MapSelectionParameters parameters)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
              () => Messenger.Default.Send<BusStopMessage>(new BusStopMessage(this)));
        }

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