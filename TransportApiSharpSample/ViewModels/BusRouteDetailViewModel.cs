using Cimbalino.Toolkit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using TransportAPISharp;
using TransportApiSharpSample.Models;
using Windows.UI.Xaml.Navigation;

namespace TransportApiSharpSample.ViewModels
{
    public class BusRouteDetailViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _routeParameters = parameter as BusRouteParameter;

            if (StopsList != null)
                StopsList = null;

            if (_routeParameters != null)
            {
                CommandBarTitle = $"{_routeParameters.DepartureTime.ToString("HH:mm")} - {_routeParameters.LineName}";
                await getRouteDetails(_routeParameters);
            }
            else
                NavigationService.GoBack();
        }

        public BusRouteDetailViewModel(IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService;
        }

        private async Task getRouteDetails(BusRouteParameter routeParameters)
        {
            if (routeParameters == null)
                throw new NullReferenceException("routeParameters");

            using (var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey))
            {
                var response = await client.BusRouteTimetable(
                    routeParameters.AtcoCode,
                    routeParameters.Direction,
                    routeParameters.LineName,
                    routeParameters.OperatorCode,
                    routeParameters.DepartureTime
                    );

                if (response == null)
                {
                    await _messageBoxService.ShowAsync(client.LastError, "Transport API error");
                    NavigationService.GoBack();
                }
                else
                    StopsList = response.Stops.ToList();
            }
        }

        /// <summary>
        /// The <see cref="CommandBarTitle"/> property's name.
        /// </summary>
        public const string CommandBarTitlePropertyName = "CommandBarTitle";

        private string _commandBarTitle = "Wibble";
        private BusRouteParameter _routeParameters;
        private IMessageBoxService _messageBoxService;

        /// <summary>
        /// Sets and gets the CommandBarTitle property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public string CommandBarTitle
        {
            get
            {
                return _commandBarTitle;
            }
            set
            {
                Set(() => CommandBarTitle, ref _commandBarTitle, value);
            }
        }

        /// <summary>
        /// The <see cref="StopsList"/> property's name.
        /// </summary>
        public const string RouteListPropertyName = "StopsList";

        private List<BusRouteTimetableStop> _stopsList = null;

        /// <summary>
        /// Sets and gets the RouteList property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public List<BusRouteTimetableStop> StopsList
        {
            get
            {
                return _stopsList;
            }
            set
            {
                Set(() => StopsList, ref _stopsList, value);
            }
        }
    }
}