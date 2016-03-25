using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using TransportAPISharp;
using Windows.UI.Xaml.Navigation;

namespace TransportApiSharpSample.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _atcoCode = parameter as string;
            await getDepartures(_atcoCode);
        }

        private async Task getDepartures(string atcoCode)
        {
            if (atcoCode == null)
                throw new ArgumentNullException("atcoCode");

            using (var client = new TransportApiClient(ApiCredentials.appId, ApiCredentials.appKey))
            {
                var response = await client.Timetable(atcoCode, DateTime.Now, true);
                if (response == null)
                    CommandBarTitle = $"Could not retrieve departures!";
                else
                {
                    CommandBarTitle = $"Next buses from {response.StopName}";
                    populateDepartures(response);
                }

                //if (response != null && response.Departures.All.Count > 0)
                //{
                //    Departures = response.Departures.All;
                //    CommandBarTitle = $"Next {Departures.Count} buses from {response.StopName}";
                //}
                //else
                //    CommandBarTitle = $"Could not retrieve departures!";
            }
        }

        private void populateDepartures(BusTimetableResponse response)
        {
            var depList = new ObservableCollection<DeparturesViewModel>();

            foreach (var item in response.Departures)
            {
                var line = new DeparturesViewModel()
                {
                    Line = item.Key,
                    Departures = new ObservableCollection<BusDeparture>(item.Value)
                };

                depList.Add(line);
            }
            DepartureList = depList;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            return base.OnNavigatedFromAsync(pageState, suspending);
        }

        /// <summary>
        /// The <see cref="DepartureList"/> property's name.
        /// </summary>
        public const string DepartureListPropertyName = "DepartureList";

        private ObservableCollection<DeparturesViewModel> _departureList = null;

        /// <summary>
        /// Sets and gets the DepartureList property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<DeparturesViewModel> DepartureList
        {
            get
            {
                return _departureList;
            }
            set
            {
                Set(() => DepartureList, ref _departureList, value);
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
        private string _atcoCode;

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
                        DepartureList.Clear();
                        await getDepartures(_atcoCode);
                    }));
            }
        }
    }
}