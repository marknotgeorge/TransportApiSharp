using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Template10.Mvvm;
using TransportAPISharp;
using TransportApiSharpSample.Models;
using TransportApiSharpSample.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TransportApiSharpSample.ViewModels
{
    public class BusStopDetailViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            switch (mode)
            {
                case NavigationMode.New:
                case NavigationMode.Refresh:
                case NavigationMode.Forward:
                    _stopParameters = parameter as BusStopParameter;
                    await getDepartures(_stopParameters);
                    break;

                case NavigationMode.Back:
                default:
                    break;
            }
        }

        public BusStopDetailViewModel(IMessageBoxService messageBoxService)
        {
            this.PropertyChanged += BusDetailPageViewModel_PropertyChanged;
            _messageBoxService = messageBoxService;
        }

        private async void BusDetailPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == UseLiveDataPropertyName)
                await Refresh();
        }

        private async Task getDepartures(BusStopParameter parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            using (var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey))
            {
                if (!(bool)UseLiveData)
                {
                    var response = await client.BusTimetable(parameters.AtcoCode, DateTime.Now, true);
                    if (response == null)
                        CommandBarTitle = $"Could not retrieve departures!";
                    else
                    {
                        CommandBarTitle = $"Next buses from {parameters.StopName}";
                        populateTimetableDepartures(response);
                    }
                }
                else
                {
                    var response = await client.BusLive(parameters.AtcoCode, true, nextBuses: true);
                    if (response == null)
                        CommandBarTitle = $"Could not retrieve departures!";
                    else
                    {
                        CommandBarTitle = $"Next buses from {parameters.StopName}";
                        populateLiveDepartures(response);
                    }
                }
            }
        }

        /// <summary>
        /// The <see cref="SelectedLiveDeparture"/> property's name.
        /// </summary>
        public const string SelectedLiveDeparturePropertyName = "SelectedLiveDeparture";

        private BusLiveDeparture _selectedLiveDeparture = null;

        /// <summary>
        /// Sets and gets the SelectedLiveDeparture property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public BusLiveDeparture SelectedLiveDeparture
        {
            get
            {
                return _selectedLiveDeparture;
            }
            set
            {
                var parameters = new BusRouteParameter()
                {
                    AtcoCode = _stopParameters.AtcoCode,
                    Direction = value.Direction,
                    LineName = value.Line,
                    OperatorCode = value.Operator
                };

                NavigationService.Navigate(typeof(BusRouteDetailPage), parameters);
            }
        }

        private RelayCommand<ItemClickEventArgs> _departureClicked;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<ItemClickEventArgs> DepartureClicked
        {
            get
            {
                return _departureClicked
                    ?? (_departureClicked = new RelayCommand<ItemClickEventArgs>(
                    (args) =>
                    {
                        var clickedTypeName = args.ClickedItem.GetType().Name;
                        BusRouteParameter parameters = null;

                        if (clickedTypeName == typeof(BusTimetableDeparture).Name)
                        {
                            var clickedDeparture = args.ClickedItem as BusTimetableDeparture;
                            if (clickedDeparture != null)
                            {
                                parameters = new BusRouteParameter()
                                {
                                    AtcoCode = _stopParameters.AtcoCode,
                                    Direction = clickedDeparture.Dir,
                                    LineName = clickedDeparture.Line,
                                    OperatorCode = clickedDeparture.Operator,
                                    DepartureTime = clickedDeparture.AimedDepartureDateTime
                                };
                            }
                        }
                        else if (clickedTypeName == typeof(BusLiveDeparture).Name)
                        {
                            _messageBoxService.ShowAsync("I can't yet show route details for live bus departures.", "Sorry!");
                        }

                        if (parameters != null)
                        {
                            NavigationService.Navigate(typeof(BusRouteDetailPage), parameters);
                        }
                    }));
            }
        }

        private void populateLiveDepartures(BusLiveResponse response)
        {
            var depList = new ObservableCollection<DeparturesViewModel<BusLiveDeparture>>();

            foreach (var item in response.Departures)
            {
                var line = new DeparturesViewModel<BusLiveDeparture>()
                {
                    Line = item.Key,
                    Departures = new ObservableCollection<BusLiveDeparture>(item.Value)
                };

                depList.Add(line);
            }
            LiveDepartureList = depList;
        }

        private void populateTimetableDepartures(BusTimetableResponse response)
        {
            var depList = new ObservableCollection<DeparturesViewModel<BusTimetableDeparture>>();

            foreach (var item in response.Departures)
            {
                var line = new DeparturesViewModel<BusTimetableDeparture>()
                {
                    Line = item.Key,
                    Departures = new ObservableCollection<BusTimetableDeparture>(item.Value)
                };

                depList.Add(line);
            }
            TimetableDepartureList = depList;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            return base.OnNavigatedFromAsync(pageState, suspending);
        }

        /// <summary>
        /// The <see cref="UseLiveData"/> property's name.
        /// </summary>
        public const string UseLiveDataPropertyName = "UseLiveData";

        private bool? _useLiveData = false;

        /// <summary>
        /// Sets and gets the UseLiveData property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public bool? UseLiveData
        {
            get
            {
                return _useLiveData;
            }
            set
            {
                Set(() => UseLiveData, ref _useLiveData, value);
            }
        }

        /// <summary>
        /// The <see cref="LiveDepartureList"/> property's name.
        /// </summary>
        public const string LiveDepartureListPropertyName = "LiveDepartureList";

        private ObservableCollection<DeparturesViewModel<BusLiveDeparture>> _liveDepartureList = null;

        /// <summary>
        /// Sets and gets the LiveDepartureList property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<DeparturesViewModel<BusLiveDeparture>> LiveDepartureList
        {
            get
            {
                return _liveDepartureList;
            }
            set
            {
                Set(() => LiveDepartureList, ref _liveDepartureList, value);
            }
        }

        /// <summary>
        /// The <see cref="TimetableDepartureList"/> property's name.
        /// </summary>
        public const string DepartureListPropertyName = "DepartureList";

        private ObservableCollection<DeparturesViewModel<BusTimetableDeparture>> _timetableDepartureList = null;

        /// <summary>
        /// Sets and gets the DepartureList property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<DeparturesViewModel<BusTimetableDeparture>> TimetableDepartureList
        {
            get
            {
                return _timetableDepartureList;
            }
            set
            {
                Set(() => TimetableDepartureList, ref _timetableDepartureList, value);
            }
        }

        /// <summary>
        /// The <see cref="CommandBarTitle"/> property's name.
        /// </summary>
        public const string CommandBarTitlePropertyName = "CommandBarTitle";

        private string _commandBarTitle = "Retrieving buses...";

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
        /// The <see cref="IsProgressRingActive"/> property's name.
        /// </summary>
        public const string IsProgressRingActivePropertyName = "IsProgressRingActive";

        private bool _isProgressRingActive = false;

        /// <summary>
        /// Sets and gets the IsProgressRingActive property. Changes to that property's value raise
        /// the PropertyChanged event.
        /// </summary>
        public bool IsProgressRingActive
        {
            get
            {
                return _isProgressRingActive;
            }
            set
            {
                Set(() => IsProgressRingActive, ref _isProgressRingActive, value);
            }
        }

        private RelayCommand _refreshDepartures;
        private BusStopParameter _stopParameters;
        private IMessageBoxService _messageBoxService;

        /// <summary>
        /// Gets the RefreshDepartures.
        /// </summary>
        public RelayCommand RefreshDepartures
        {
            get
            {
                return _refreshDepartures
                    ?? (_refreshDepartures = new RelayCommand(
                    async () =>
                    {
                        await Refresh();
                    }));
            }
        }

        private async Task Refresh()
        {
            if (TimetableDepartureList != null)
                TimetableDepartureList.Clear();
            if (LiveDepartureList != null)
                LiveDepartureList.Clear();
            await getDepartures(_stopParameters);
        }
    }
}