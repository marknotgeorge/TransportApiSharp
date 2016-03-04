using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using TransportApiSharpSampleXamarin.Services;
using TransportApiSharpSampleXamarin.ViewModels;
using Xamarin.Forms;

namespace TransportApiSharpSampleXamarin
{
    public class App : Application
    {
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }

        public App()
        {
            var navService = new NavigationService();
            navService.Configure(ViewModelLocator.MainPage, typeof(MainPage));
            SimpleIoc.Default.Register<INavigationService>(() => navService);

            var mainPage = new NavigationPage(new MainPage());
            navService.Initialize(mainPage);

            MainPage = mainPage;
        }

        public static Page GetMainPage()
        {
            return new MainPage();
        }
    }
}