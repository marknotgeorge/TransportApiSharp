using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using TransportAPISharp;
using TransportApiSharpSample;
using TransportApiSharpSample.Messages;
using TransportApiSharpSample.Models;
using TransportApiSharpSample.Views;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using WpWinNl.Maps;

namespace TransportApiSharpSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ILocationService _locationService;
        private List<BasicGeoposition> positions;

        public MainPageViewModel(
            ILocationService locationService,
            IStatusBarService statusBarService)
        {
            _locationService = locationService;
            _statusBarService = statusBarService;
            MapCenter = new Geopoint(new BasicGeoposition { Latitude = 0, Longitude = 0 });
            NearbyBusStops = new ObservableCollection<BusStop>();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (mode != NavigationMode.Back)
                ExecuteLoadBusStops();
            else
            {
                if (positions != null)
                {
                    MapArea = GeoboundingBox.TryCompute(positions);
                    if (MapArea != null)
                        Messenger.Default.Send<GeoboundingBoxMessage>(new GeoboundingBoxMessage(MapArea));
                }
            }
            Messenger.Default.Register<BusStopMessage>(this, (message) =>
            {
                Debug.WriteLine($"Bus stop selected: {message.Payload.Title}");
                var parameters = new BusStopParameter(message.Payload.Title, message.Payload.AtcoCode);

                NavigationService.Navigate(typeof(BusStopDetailPage), parameters);
            });
            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
            }
            Messenger.Default.Unregister<BusStopMessage>(this);

            return Task.CompletedTask;
        }

        /// <summary>
        /// The <see cref="IsBusChecked"/> property's name.
        /// </summary>
        public const string IsBusCheckedPropertyName = "IsBusChecked";

        private bool? _isBusChecked = true;

        /// <summary>
        /// Sets and gets the IsBusChecked property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool? IsBusChecked
        {
            get
            {
                return _isBusChecked;
            }

            set
            {
                if (_isBusChecked == value)
                {
                    return;
                }

                if (value.HasValue && value == true)
                {
                    Mode = TransportMode.Bus;
                    IsTrainChecked = false;
                    IsTubeChecked = false;
                }

                _isBusChecked = value;
                RaisePropertyChanged(IsBusCheckedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsTrainChecked"/> property's name.
        /// </summary>
        public const string IsTrainCheckedPropertyName = "IsTrainChecked";

        private bool? _isTrainChecked = false;

        /// <summary>
        /// Sets and gets the IsTrainChecked property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool? IsTrainChecked
        {
            get
            {
                return _isTrainChecked;
            }

            set
            {
                if (_isTrainChecked == value)
                {
                    return;
                }

                if (value.HasValue && value == true)
                {
                    Mode = TransportMode.Train;
                    IsTubeChecked = false;
                    IsBusChecked = false;
                }

                _isTrainChecked = value;
                RaisePropertyChanged(IsTrainCheckedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsTubeChecked"/> property's name.
        /// </summary>
        public const string IsTubeCheckedPropertyName = "IsTubeChecked";

        private bool? _isTubeChecked = false;

        /// <summary>
        /// Sets and gets the IsTubeChecked property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool? IsTubeChecked
        {
            get
            {
                return _isTubeChecked;
            }

            set
            {
                if (_isTubeChecked == value)
                {
                    return;
                }

                if (value.HasValue && value == true)
                {
                    Mode = TransportMode.Tube;
                    IsBusChecked = false;
                    IsTrainChecked = false;
                }

                _isTubeChecked = value;
                RaisePropertyChanged(IsTubeCheckedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Mode"/> property's name.
        /// </summary>
        public const string ModePropertyName = "Mode";

        private TransportMode _mode = TransportMode.Bus;

        /// <summary>
        /// Sets and gets the Mode property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public TransportMode Mode
        {
            get
            {
                return _mode;
            }

            set
            {
                if (_mode == value)
                {
                    return;
                }

                _mode = value;
                RaisePropertyChanged(ModePropertyName);
            }
        }

        private RelayCommand<MapInputEventArgs> _resetPosition;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<MapInputEventArgs> ResetPosition
        {
            get
            {
                return _resetPosition
                    ?? (_resetPosition = new RelayCommand<MapInputEventArgs>(ExecuteResetPosition));
            }
        }

        private async void ExecuteResetPosition(MapInputEventArgs args)
        {
            if (args != null)
            {
                MapCenter = args.Location;
                await CreateBusStops(args.Location.Position);
            }
        }

        /// <summary>
        /// The <see cref="NearbyBusStops"/> property's name.
        /// </summary>
        public const string NearbyBusStopsPropertyName = "NearbyBusStops";

        private ObservableCollection<BusStop> _nearbyBusStops = null;

        /// <summary>
        /// Sets and gets the NearbyBusStops property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<BusStop> NearbyBusStops
        {
            get
            {
                return _nearbyBusStops;
            }

            set
            {
                if (_nearbyBusStops == value)
                {
                    return;
                }

                _nearbyBusStops = value;
                RaisePropertyChanged(NearbyBusStopsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsProgressRingActive"/> property's name.
        /// </summary>
        public const string IsProgressRingActivePropertyName = "IsProgressRingActive";

        private bool _myProperty = false;

        /// <summary>
        /// Sets and gets the IsProgressRingActive property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsProgressRingActive
        {
            get
            {
                return _myProperty;
            }
            set
            {
                Set(() => IsProgressRingActive, ref _myProperty, value);
            }
        }

        /// <summary>
        /// The <see cref="CommandBarTitle"/> property's name.
        /// </summary>
        public const string CommandBarTitlePropertyName = "CommandBarTitle";

        private string _commandBarTitle = "Nearby Bus Stops";

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
        /// The <see cref="MapCenter"/> property's name.
        /// </summary>
        public const string MapCenterPropertyName = "MapCenter";

        private Geopoint _mapCenter;

        /// <summary>
        /// Sets and gets the MapCenter property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public Geopoint MapCenter
        {
            get
            {
                return _mapCenter;
            }

            set
            {
                if (_mapCenter == value)
                {
                    return;
                }

                _mapCenter = value;
                RaisePropertyChanged(MapCenterPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MapZoomLevel"/> property's name.
        /// </summary>
        public const string MapZoomLevelPropertyName = "MapZoomLevel";

        private int _mapZoomLevel = 0;

        /// <summary>
        /// Sets and gets the MapZoomLevel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int MapZoomLevel
        {
            get
            {
                return _mapZoomLevel;
            }

            set
            {
                Set(() => MapZoomLevel, ref _mapZoomLevel, value);
            }
        }

        /// <summary>
        /// The <see cref="MapArea"/> property's name.
        /// </summary>
        public const string MapAreaPropertyName = "MapArea";

        private GeoboundingBox _mapArea;

        /// <summary>
        /// Sets and gets the MapArea property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public GeoboundingBox MapArea
        {
            get
            {
                return _mapArea;
            }

            set
            {
                Set(() => MapArea, ref _mapArea, value);
            }
        }

        private RelayCommand _LoadBusStops;
        private IStatusBarService _statusBarService;

        /// <summary>
        /// Gets the LoadBusStops.
        /// </summary>
        public RelayCommand LoadBusStops
        {
            get
            {
                return _LoadBusStops
                    ?? (_LoadBusStops = new RelayCommand(ExecuteLoadBusStops));
            }
        }

        private async void ExecuteLoadBusStops()
        {
            var positions = new List<BasicGeoposition>();
            await _statusBarService.ShowAsync("Loading Bus Stops...", true);

            var location = await _locationService.GetPositionAsync();

            if (location != null)
            {
                var locationBGP = new BasicGeoposition()
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                };
                await CreateBusStops(locationBGP);
            }

            if (App.OperatorCodes == null)
            {
                await _statusBarService.ShowAsync("Loading Bus Operators...", true);
                App.OperatorCodes = await getOperatorCodes();
            }

            await _statusBarService.HideAsync();
        }

        private async Task<List<BusOperator>> getOperatorCodes()
        {
            using (var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey))
            {
                return await client.GetBusOperators();
            }
        }

        private async Task CreateBusStops(BasicGeoposition location)
        {
            positions = new List<BasicGeoposition>();
            NearbyBusStops.Clear();
            var youAreHere = new BusStop(
                                location.Latitude, location.Longitude,
                                "You are here!", "",
                                RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/YouAreHere.png")));

            NearbyBusStops.Add(youAreHere);

            positions.Add(youAreHere.Point);

            using (var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey))
            {
                var response = await client.BusStopsNear(location.Latitude, location.Longitude);

                if (response != null)
                {
                    foreach (var item in response.stops)
                    {
                        var point = new BusStop(item.latitude, item.longitude,
                            item.name, item.atcocode,
                            RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/BusPin.png")));
                        NearbyBusStops.Add(point);
                        positions.Add(point.Point);
                    }

                    MapArea = GeoboundingBox.TryCompute(positions);
                    Messenger.Default.Send<GeoboundingBoxMessage>(new GeoboundingBoxMessage(MapArea));
                }
            }
        }
    }
}