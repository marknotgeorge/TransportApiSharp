using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using TransportApiSharpSample.Models;
using Windows.UI.Xaml.Navigation;

namespace TransportApiSharpSample.ViewModels
{
    public class BusRouteDetailViewModel : ViewModelBase
    {
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _routeParameters = parameter as BusRouteParameter;

            if (_routeParameters != null)
            {
                CommandBarTitle = _routeParameters.LineName;
            }
            else
                NavigationService.GoBack();

            return Task.CompletedTask;
        }

        /// <summary>
        /// The <see cref="CommandBarTitle"/> property's name.
        /// </summary>
        public const string CommandBarTitlePropertyName = "CommandBarTitle";

        private string _commandBarTitle = "Wibble";
        private BusRouteParameter _routeParameters;

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
    }
}