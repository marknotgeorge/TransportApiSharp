using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace TransportApiSharpSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ILocationService _locationService;

        public MainPageViewModel(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
            }
            return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
        }

        private RelayCommand _LoadBusStops;

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

        private void ExecuteLoadBusStops()
        {
        }
    }
}