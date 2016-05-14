using Cimbalino.Toolkit.Services;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportApiSharpSample.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ILocationService, LocationService>();
            SimpleIoc.Default.Register<IStatusBarService, StatusBarService>();

            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<BusStopDetailViewModel>();
            SimpleIoc.Default.Register<BusRouteDetailViewModel>();
        }

        public MainPageViewModel MainPageViewModel
            => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        public BusStopDetailViewModel BusStopDetailViewModel => ServiceLocator.Current.GetInstance<BusStopDetailViewModel>();

        public BusRouteDetailViewModel BusRouteDetailViewModel => ServiceLocator.Current.GetInstance<BusRouteDetailViewModel>();
    }
}