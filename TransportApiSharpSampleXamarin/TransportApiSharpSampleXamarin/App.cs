using System;
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
            MainPage = new NavigationPage(new MainPage());
        }

        public static Page GetMainPage()
        {
            return new MainPage();
        }
    }
}